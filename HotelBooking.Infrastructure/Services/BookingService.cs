using AutoMapper;
using HotelBooking.Application.DTOs.Booking;
using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Email;
using HotelBooking.Application.DTOs.Promotion;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class BookingService(
    IUnitOfWork uow,
    IMapper mapper,
    IEmailService emailService,
    ILoyaltyService loyaltyService, IPromotionService promotionService) : IBookingService
{
    public async Task<BookingDto> CreateAsync(
    CreateBookingRequest request, Guid userId, CancellationToken ct = default)
    {
        var room = await uow.Rooms.GetWithHotelAsync(request.RoomId, ct)
            ?? throw new NotFoundException(nameof(Room), request.RoomId);

        var isAvailable = await uow.Bookings.IsRoomAvailableAsync(
            request.RoomId, request.CheckInDate, request.CheckOutDate, ct);
        if (!isAvailable)
            throw new ConflictException("Room is not available for the selected dates.");

        var nights = (request.CheckOutDate - request.CheckInDate).Days;
        var originalPrice = room.PricePerNight * nights;

        // ── Apply promo code if provided ──────────────────────────
        decimal discountAmount = 0;
        decimal totalPrice = originalPrice;
        string? appliedPromo = null;

        if (!string.IsNullOrWhiteSpace(request.PromoCode))
        {
            var promoResult = await promotionService.ApplyAsync(new ApplyPromoRequest
            {
                Code = request.PromoCode,
                BookingAmount = originalPrice
            }, ct);

            if (!promoResult.IsValid)
                throw new ValidationException($"Promo code error: {promoResult.Message}");

            discountAmount = promoResult.DiscountAmount;
            totalPrice = promoResult.FinalAmount;
            appliedPromo = request.PromoCode.ToUpper();

            // Increment usage count
            var promo = await uow.Promotions.GetByCodeAsync(request.PromoCode, ct);
            if (promo != null)
            {
                promo.CurrentUsageCount++;
                uow.Promotions.Update(promo);
            }
        }
        // ─────────────────────────────────────────────────────────

        var booking = mapper.Map<Booking>(request);
        booking.UserId = userId;
        booking.HotelId = room.HotelId;
        booking.OriginalPrice = originalPrice;
        booking.DiscountAmount = discountAmount;
        booking.TotalPrice = totalPrice;
        booking.PromoCode = appliedPromo;
        booking.Status = BookingStatus.Confirmed;

        await uow.Bookings.AddAsync(booking, ct);
        await uow.SaveChangesAsync(ct);

        // Earn loyalty points on the final price paid
        await loyaltyService.EarnPointsAsync(
            userId.ToString(), booking.Id, booking.TotalPrice, ct);

        // Send confirmation email
        await emailService.SendBookingConfirmationAsync(new BookingConfirmationEmailDto
        {
            ToEmail = room.Hotel.Email,
            GuestName = userId.ToString(),
            ConfirmationNumber = booking.ConfirmationNumber,
            HotelName = room.Hotel.Name,
            RoomNumber = room.RoomNumber,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            TotalPrice = booking.TotalPrice
        }, ct);

        return mapper.Map<BookingDto>(booking);
    }

    public async Task<BookingDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var booking = await uow.Bookings.GetWithDetailsAsync(id, ct)
            ?? throw new NotFoundException(nameof(Booking), id);
        return mapper.Map<BookingDto>(booking);
    }

    public async Task<PagedResult<BookingDto>> GetUserBookingsAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var result = await uow.Bookings.GetByUserIdAsync(userId, page, pageSize, ct);
        return new PagedResult<BookingDto>
        {
            Items = mapper.Map<IEnumerable<BookingDto>>(result.Items),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<BookingDto> CancelAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var booking = await uow.Bookings.GetWithDetailsAsync(id, ct)
            ?? throw new NotFoundException(nameof(Booking), id);

        if (booking.UserId != userId)
            throw new ForbiddenException("You cannot cancel another user's booking.");

        if (booking.Status == BookingStatus.Cancelled)
            throw new ConflictException("Booking is already cancelled.");

        booking.Status = BookingStatus.Cancelled;
        uow.Bookings.Update(booking);
        await uow.SaveChangesAsync(ct);

        await emailService.SendCancellationEmailAsync(new CancellationEmailDto
        {
            ToEmail = string.Empty,
            GuestName = userId.ToString(),
            ConfirmationNumber = booking.ConfirmationNumber,
            HotelName = booking.Room.Hotel.Name,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate
        }, ct);

        return mapper.Map<BookingDto>(booking);
    }

    public async Task<BookingDto> RebookAsync(
        Guid bookingId, Guid userId, CreateBookingRequest request, CancellationToken ct = default)
    {
        var original = await uow.Bookings.GetWithDetailsAsync(bookingId, ct)
            ?? throw new NotFoundException(nameof(Booking), bookingId);

        if (original.UserId != userId)
            throw new ForbiddenException("Access denied.");

        // Reuse same room if not specified
        if (request.RoomId == Guid.Empty)
            request = new CreateBookingRequest
            {
                RoomId = original.RoomId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                SpecialRequests = request.SpecialRequests,
                PromoCode = request.PromoCode
            };

        return await CreateAsync(request, userId, ct);
    }
}