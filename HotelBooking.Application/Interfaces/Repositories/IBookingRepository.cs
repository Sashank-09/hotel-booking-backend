using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<PagedResult<Booking>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
    Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken ct = default);
    Task<Booking?> GetByConfirmationNumberAsync(string confirmationNumber, CancellationToken ct = default);
    Task<Booking?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
}