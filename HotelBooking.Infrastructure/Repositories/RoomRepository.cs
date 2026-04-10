using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class RoomRepository(AppDbContext context)
    : GenericRepository<Room>(context), IRoomRepository
{
    public async Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default) =>
        await _set
            .Include(r => r.Amenities)
            .Where(r => r.HotelId == hotelId)
            .ToListAsync(ct);

    public async Task<Room?> GetWithHotelAsync(Guid roomId, CancellationToken ct = default) =>
        await _set
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == roomId, ct);

    public async Task<Room?> GetWithAmenitiesAsync(Guid roomId, CancellationToken ct = default) =>
        await _set
            .Include(r => r.Amenities)
            .FirstOrDefaultAsync(r => r.Id == roomId, ct);
}