using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.User;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>Get the authenticated user's profile.</summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var result = await userService.GetProfileAsync(CurrentUserId, ct);
        return Ok(ApiResponse<UserProfileDto>.Ok(result));
    }

    /// <summary>Update the authenticated user's profile.</summary>
    [HttpPut("profile")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileDto dto, CancellationToken ct)
    {
        var result = await userService.UpdateProfileAsync(CurrentUserId, dto, ct);
        return Ok(ApiResponse<UserProfileDto>.Ok(result, "Profile updated successfully."));
    }

    /// <summary>Change the authenticated user's password.</summary>
    [HttpPut("change-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordDto dto, CancellationToken ct)
    {
        await userService.ChangePasswordAsync(CurrentUserId, dto, ct);
        return Ok(ApiResponse<object>.Ok(null!, "Password changed successfully."));
    }
}