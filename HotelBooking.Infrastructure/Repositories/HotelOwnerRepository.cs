using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class HotelOwnerRepository(AppDbContext context)
    : GenericRepository<HotelOwnerProfile>(context), IHotelOwnerRepository
{
    public async Task<HotelOwnerProfile?> GetByOwnerIdAsync(string ownerId, CancellationToken ct = default) =>
        await _set.FirstOrDefaultAsync(h => h.OwnerId == ownerId, ct);

    public async Task<bool> IsVerifiedAsync(string ownerId, CancellationToken ct = default) =>
        await _set.AnyAsync(h => h.OwnerId == ownerId && h.IsVerified, ct);
}