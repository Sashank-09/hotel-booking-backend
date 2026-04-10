using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Promotion;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController(IPromotionService promotionService) : ControllerBase
{
    /// <summary>Get all currently active promotions.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PromotionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await promotionService.GetActiveAsync(ct);
        return Ok(ApiResponse<IEnumerable<PromotionDto>>.Ok(result));
    }

    /// <summary>Get all promotions for a specific hotel.</summary>
    [HttpGet("hotel/{hotelId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PromotionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHotel(Guid hotelId, CancellationToken ct)
    {
        var result = await promotionService.GetByHotelIdAsync(hotelId, ct);
        return Ok(ApiResponse<IEnumerable<PromotionDto>>.Ok(result));
    }

    /// <summary>Apply a promo code to a booking amount.</summary>
    [HttpPost("apply")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ApplyPromoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Apply(
        [FromBody] ApplyPromoRequest request, CancellationToken ct)
    {
        var result = await promotionService.ApplyAsync(request, ct);
        return Ok(ApiResponse<ApplyPromoResponse>.Ok(result));
    }

    /// <summary>Create a new promotion. [Admin only]</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PromotionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePromotionRequest request, CancellationToken ct)
    {
        var result = await promotionService.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<PromotionDto>.Ok(result, "Promotion created successfully."));
    }

    /// <summary>Deactivate a promotion. [Admin only]</summary>
    [HttpPut("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await promotionService.DeactivateAsync(id, ct);
        return Ok(ApiResponse<object>.Ok(null!, "Promotion deactivated."));
    }
}