using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class LoyaltyRepository(AppDbContext context)
    : GenericRepository<LoyaltyTransaction>(context), ILoyaltyRepository
{
    public async Task<IEnumerable<LoyaltyTransaction>> GetByUserIdAsync(
        string userId, CancellationToken ct = default) =>
        await _set
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(ct);

    public async Task<int> GetUserPointsBalanceAsync(string userId, CancellationToken ct = default)
    {
        var transactions = await _set
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync(ct);

        return transactions?.BalanceAfter ?? 0;
    }
}