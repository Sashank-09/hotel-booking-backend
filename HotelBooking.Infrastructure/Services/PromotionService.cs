using AutoMapper;
using HotelBooking.Application.DTOs.Promotion;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class PromotionService(IUnitOfWork uow, IMapper mapper) : IPromotionService
{
    public async Task<PromotionDto> CreateAsync(CreatePromotionRequest request, CancellationToken ct = default)
    {
        var existing = await uow.Promotions.GetByCodeAsync(request.Code, ct);
        if (existing != null)
            throw new ConflictException($"Promo code '{request.Code}' already exists.");

        var promo = mapper.Map<Promotion>(request);
        await uow.Promotions.AddAsync(promo, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<PromotionDto>(promo);
    }

    public async Task<IEnumerable<PromotionDto>> GetActiveAsync(CancellationToken ct = default)
    {
        var promos = await uow.Promotions.GetActivePromotionsAsync(ct);
        return mapper.Map<IEnumerable<PromotionDto>>(promos);
    }

    public async Task<ApplyPromoResponse> ApplyAsync(ApplyPromoRequest request, CancellationToken ct = default)
    {
        var promo = await uow.Promotions.GetByCodeAsync(request.Code, ct);

        if (promo == null || !promo.IsActive)
            return new ApplyPromoResponse { IsValid = false, Message = "Invalid or inactive promo code." };

        if (promo.ValidTo < DateTime.UtcNow)
            return new ApplyPromoResponse { IsValid = false, Message = "Promo code has expired." };

        if (promo.MaxUsageCount.HasValue && promo.CurrentUsageCount >= promo.MaxUsageCount)
            return new ApplyPromoResponse { IsValid = false, Message = "Promo code usage limit reached." };

        if (promo.MinimumBookingAmount.HasValue && request.BookingAmount < promo.MinimumBookingAmount)
            return new ApplyPromoResponse { IsValid = false, Message = $"Minimum booking amount is {promo.MinimumBookingAmount:C}." };

        var discount = promo.Type == PromoType.Percentage
            ? request.BookingAmount * (promo.DiscountValue / 100)
            : promo.DiscountValue;

        // Cap discount — can never exceed the booking amount
        discount = Math.Min(discount, request.BookingAmount);
        var finalAmount = request.BookingAmount - discount;

        return new ApplyPromoResponse
        {
            IsValid = true,
            DiscountAmount = Math.Round(discount, 2),
            FinalAmount = Math.Round(finalAmount, 2),
            Message = $"Promo '{promo.Code}' applied! You save {discount:C}."
        };
    }
    //async Task<ApplyPromoResponse> ApplyAsync(ApplyPromoRequest request, CancellationToken ct = default)
    //{
    //    var promo = await uow.Promotions.GetByCodeAsync(request.Code, ct);

    //    if (promo == null || !promo.IsActive)
    //        return new ApplyPromoResponse { IsValid = false, Message = "Invalid or inactive promo code." };

    //    if (promo.ValidTo < DateTime.UtcNow)
    //        return new ApplyPromoResponse { IsValid = false, Message = "Promo code has expired." };

    //    if (promo.MaxUsageCount.HasValue && promo.CurrentUsageCount >= promo.MaxUsageCount)
    //        return new ApplyPromoResponse { IsValid = false, Message = "Promo code usage limit reached." };

    //    if (promo.MinimumBookingAmount.HasValue && request.BookingAmount < promo.MinimumBookingAmount)
    //        return new ApplyPromoResponse { IsValid = false, Message = $"Minimum booking amount is {promo.MinimumBookingAmount}." };

    //    var discount = promo.Type == PromoType.Percentage
    //        ? request.BookingAmount * (promo.DiscountValue / 100)
    //        : promo.DiscountValue;

    //    var finalAmount = Math.Max(0, request.BookingAmount - discount);

    //    return new ApplyPromoResponse
    //    {
    //        IsValid = true,
    //        DiscountAmount = discount,
    //        FinalAmount = finalAmount,
    //        Message = $"Promo applied! You save {discount:C}."
    //    };
    //}

    public async Task<IEnumerable<PromotionDto>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default)
    {
        var promos = await uow.Promotions.GetByHotelIdAsync(hotelId, ct);
        return mapper.Map<IEnumerable<PromotionDto>>(promos);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var promo = await uow.Promotions.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Promotion), id);
        promo.IsActive = false;
        uow.Promotions.Update(promo);
        await uow.SaveChangesAsync(ct);
    }
}