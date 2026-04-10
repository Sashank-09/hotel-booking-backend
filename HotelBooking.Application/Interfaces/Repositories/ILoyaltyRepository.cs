using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface ILoyaltyRepository : IGenericRepository<LoyaltyTransaction>
{
    Task<IEnumerable<LoyaltyTransaction>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<int> GetUserPointsBalanceAsync(string userId, CancellationToken ct = default);
}