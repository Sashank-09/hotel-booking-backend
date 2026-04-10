using AutoMapper;
using HotelBooking.Application.DTOs.Room;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class RoomService(IUnitOfWork uow, IMapper mapper) : IRoomService
{
    public async Task<IEnumerable<RoomDto>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default)
    {
        var rooms = await uow.Rooms.GetByHotelIdAsync(hotelId, ct);
        return mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<RoomDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var room = await uow.Rooms.GetWithAmenitiesAsync(id, ct)
            ?? throw new NotFoundException(nameof(Room), id);
        return mapper.Map<RoomDto>(room);
    }

    public async Task<RoomDto> CreateAsync(CreateRoomRequest request, CancellationToken ct = default)
    {
        var hotelExists = await uow.Hotels.ExistsAsync(h => h.Id == request.HotelId, ct);
        if (!hotelExists)
            throw new NotFoundException(nameof(Hotel), request.HotelId);

        var room = mapper.Map<Room>(request);
        room.Amenities = request.Amenities
            .Select(a => new RoomAmenity { Name = a })
            .ToList();

        await uow.Rooms.AddAsync(room, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<RoomDto>(room);
    }

    public async Task<RoomDto> UpdateAsync(Guid id, CreateRoomRequest request, CancellationToken ct = default)
    {
        var room = await uow.Rooms.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Room), id);
        mapper.Map(request, room);
        uow.Rooms.Update(room);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<RoomDto>(room);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var room = await uow.Rooms.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Room), id);
        room.IsDeleted = true;
        uow.Rooms.Update(room);
        await uow.SaveChangesAsync(ct);
    }
}