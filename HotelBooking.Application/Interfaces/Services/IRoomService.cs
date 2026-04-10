using HotelBooking.Application.DTOs.Room;

namespace HotelBooking.Application.Interfaces.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default);
    Task<RoomDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RoomDto> CreateAsync(CreateRoomRequest request, CancellationToken ct = default);
    Task<RoomDto> UpdateAsync(Guid id, CreateRoomRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}