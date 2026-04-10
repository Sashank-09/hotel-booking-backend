using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Hotel;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IHotelRepository : IGenericRepository<Hotel>
{
    Task<PagedResult<Hotel>> SearchAsync(SearchHotelRequest request, CancellationToken ct = default);
    Task<Hotel?> GetWithRoomsAsync(Guid id, CancellationToken ct = default);
    Task<Hotel?> GetWithReviewsAsync(Guid id, CancellationToken ct = default);
}