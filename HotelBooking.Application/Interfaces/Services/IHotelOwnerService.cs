using HotelBooking.Application.DTOs.HotelOwner;

namespace HotelBooking.Application.Interfaces.Services;

public interface IHotelOwnerService
{
    Task<HotelOwnerDto> CreateProfileAsync(CreateHotelOwnerRequest request, CancellationToken ct = default);
    Task<HotelOwnerDto> GetByOwnerIdAsync(string ownerId, CancellationToken ct = default);
    Task<HotelOwnerDto> VerifyOwnerAsync(string ownerId, CancellationToken ct = default);
}