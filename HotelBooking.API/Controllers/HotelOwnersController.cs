using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.HotelOwner;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HotelOwnersController(IHotelOwnerService hotelOwnerService) : ControllerBase
{
    /// <summary>Create a hotel owner profile. [Authenticated]</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<HotelOwnerDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateHotelOwnerRequest request, CancellationToken ct)
    {
        var result = await hotelOwnerService.CreateProfileAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<HotelOwnerDto>.Ok(result, "Hotel owner profile created."));
    }

    /// <summary>Get a hotel owner profile by owner ID. [Admin only]</summary>
    [HttpGet("{ownerId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<HotelOwnerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByOwnerId(string ownerId, CancellationToken ct)
    {
        var result = await hotelOwnerService.GetByOwnerIdAsync(ownerId, ct);
        return Ok(ApiResponse<HotelOwnerDto>.Ok(result));
    }

    /// <summary>Verify a hotel owner. [Admin only]</summary>
    [HttpPut("{ownerId}/verify")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<HotelOwnerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Verify(string ownerId, CancellationToken ct)
    {
        var result = await hotelOwnerService.VerifyOwnerAsync(ownerId, ct);
        return Ok(ApiResponse<HotelOwnerDto>.Ok(result, "Owner verified successfully."));
    }
}