using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IHotelOwnerRepository : IGenericRepository<HotelOwnerProfile>
{
    Task<HotelOwnerProfile?> GetByOwnerIdAsync(string ownerId, CancellationToken ct = default);
    Task<bool> IsVerifiedAsync(string ownerId, CancellationToken ct = default);
}