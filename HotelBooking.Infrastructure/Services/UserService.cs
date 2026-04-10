using AutoMapper;
using HotelBooking.Application.DTOs.User;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Infrastructure.Services;

public class UserService(
    UserManager<AppUser> userManager,
    ILoyaltyService loyaltyService) : IUserService
{
    public async Task<UserProfileDto> GetProfileAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User", userId);

        var roles = await userManager.GetRolesAsync(user);
        var loyalty = await loyaltyService.GetSummaryAsync(userId, ct);

        return new UserProfileDto
        {
            Id = user.Id.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            ProfilePicture = user.ProfilePicture,
            Roles = roles,
            LoyaltyPoints = loyalty.TotalPoints,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User", userId);

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.ProfilePicture = dto.ProfilePicture;

        await userManager.UpdateAsync(user);
        return await GetProfileAsync(userId, ct);
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User", userId);

        var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}