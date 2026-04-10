using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Loyalty;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoyaltyController(ILoyaltyService loyaltyService) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>Get the authenticated user's loyalty points summary.</summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(ApiResponse<LoyaltySummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
    {
        var result = await loyaltyService.GetSummaryAsync(CurrentUserId, ct);
        return Ok(ApiResponse<LoyaltySummaryDto>.Ok(result));
    }

    /// <summary>Redeem loyalty points against a booking. [Authenticated]</summary>
    [HttpPost("redeem")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Redeem(
        [FromBody] RedeemPointsRequest request, CancellationToken ct)
    {
        await loyaltyService.RedeemPointsAsync(CurrentUserId, request.Points, request.BookingId, ct);
        return Ok(ApiResponse<object>.Ok(null!, $"{request.Points} points redeemed successfully."));
    }
}