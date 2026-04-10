using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class PromotionRepository(AppDbContext context)
    : GenericRepository<Promotion>(context), IPromotionRepository
{
    public async Task<Promotion?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        await _set.FirstOrDefaultAsync(p => p.Code == code, ct);

    public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync(CancellationToken ct = default) =>
        await _set
            .Where(p => p.IsActive && p.ValidFrom <= DateTime.UtcNow && p.ValidTo >= DateTime.UtcNow)
            .ToListAsync(ct);

    public async Task<IEnumerable<Promotion>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default) =>
        await _set
            .Where(p => p.HotelId == hotelId)
            .ToListAsync(ct);
}