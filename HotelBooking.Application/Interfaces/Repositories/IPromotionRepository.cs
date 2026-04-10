using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IPromotionRepository : IGenericRepository<Promotion>
{
    Task<Promotion?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IEnumerable<Promotion>> GetActivePromotionsAsync(CancellationToken ct = default);
    Task<IEnumerable<Promotion>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default);
}