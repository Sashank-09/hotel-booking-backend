using HotelBooking.Application.DTOs.User;

namespace HotelBooking.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(string userId, CancellationToken ct = default);
    Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken ct = default);
    Task ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken ct = default);
}