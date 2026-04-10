using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class BookingRepository(AppDbContext context)
    : GenericRepository<Booking>(context), IBookingRepository
{
    public async Task<PagedResult<Booking>> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _set
            .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Booking>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public async Task<bool> IsRoomAvailableAsync(
        Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken ct = default) =>
        !await _set.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            b.CheckInDate < checkOut &&
            b.CheckOutDate > checkIn, ct);

    public async Task<Booking?> GetByConfirmationNumberAsync(
        string confirmationNumber, CancellationToken ct = default) =>
        await _set
            .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.ConfirmationNumber == confirmationNumber, ct);

    public async Task<Booking?> GetWithDetailsAsync(Guid id, CancellationToken ct = default) =>
        await _set
            .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
}