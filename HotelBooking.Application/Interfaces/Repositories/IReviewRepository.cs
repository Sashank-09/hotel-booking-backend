using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IEnumerable<Review>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default);
    Task<double> GetAverageRatingAsync(Guid hotelId, CancellationToken ct = default);
}