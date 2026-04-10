using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class ReviewRepository(AppDbContext context)
    : GenericRepository<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default) =>
        await _set
            .Where(r => r.HotelId == hotelId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    public async Task<double> GetAverageRatingAsync(Guid hotelId, CancellationToken ct = default)
    {
        var ratings = await _set
            .Where(r => r.HotelId == hotelId)
            .Select(r => r.Rating)
            .ToListAsync(ct);

        return ratings.Any() ? ratings.Average() : 0;
    }
}