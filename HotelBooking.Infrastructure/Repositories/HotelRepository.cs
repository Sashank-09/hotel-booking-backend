using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Hotel;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class HotelRepository(AppDbContext context)
    : GenericRepository<Hotel>(context), IHotelRepository
{
    public async Task<PagedResult<Hotel>> SearchAsync(SearchHotelRequest request, CancellationToken ct = default)
    {
        var query = _set
            .Include(h => h.Rooms)
            .Where(h => h.IsActive);

        if (!string.IsNullOrWhiteSpace(request.City))
            query = query.Where(h => h.City.Contains(request.City));

        if (!string.IsNullOrWhiteSpace(request.Location))
            query = query.Where(h => h.Location.Contains(request.Location));

        if (request.StarRating.HasValue)
            query = query.Where(h => h.StarRating == request.StarRating.Value);

        if (request.MinPrice.HasValue)
            query = query.Where(h => h.Rooms.Any(r => r.PricePerNight >= request.MinPrice.Value));

        if (request.MaxPrice.HasValue)
            query = query.Where(h => h.Rooms.Any(r => r.PricePerNight <= request.MaxPrice.Value));

        if (request.Guests.HasValue)
            query = query.Where(h => h.Rooms.Any(r => r.MaxOccupancy >= request.Guests.Value));

        if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
            query = query.Where(h => h.Rooms.Any(r =>
                !r.Bookings.Any(b =>
                    b.Status != Domain.Enums.BookingStatus.Cancelled &&
                    b.CheckInDate < request.CheckOutDate &&
                    b.CheckOutDate > request.CheckInDate)));

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<Hotel>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total
        };
    }

    public async Task<Hotel?> GetWithRoomsAsync(Guid id, CancellationToken ct = default) =>
        await _set
            .Include(h => h.Rooms)
                .ThenInclude(r => r.Amenities)
            .FirstOrDefaultAsync(h => h.Id == id, ct);

    public async Task<Hotel?> GetWithReviewsAsync(Guid id, CancellationToken ct = default) =>
        await _set
            .Include(h => h.Reviews)
            .FirstOrDefaultAsync(h => h.Id == id, ct);
}