using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default);
    Task<Room?> GetWithHotelAsync(Guid roomId, CancellationToken ct = default);
    Task<Room?> GetWithAmenitiesAsync(Guid roomId, CancellationToken ct = default);
}