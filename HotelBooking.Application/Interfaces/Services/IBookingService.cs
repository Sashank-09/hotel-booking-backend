using HotelBooking.Application.DTOs.Booking;
using HotelBooking.Application.DTOs.Common;

namespace HotelBooking.Application.Interfaces.Services;

public interface IBookingService
{
    Task<BookingDto> CreateAsync(CreateBookingRequest request, Guid userId, CancellationToken ct = default);
    Task<BookingDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<BookingDto>> GetUserBookingsAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
    Task<BookingDto> CancelAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<BookingDto> RebookAsync(Guid bookingId, Guid userId, CreateBookingRequest request, CancellationToken ct = default);
}