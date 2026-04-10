using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Review;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get all reviews for a hotel.</summary>
    [HttpGet("hotel/{hotelId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReviewDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHotel(Guid hotelId, CancellationToken ct)
    {
        var result = await reviewService.GetByHotelIdAsync(hotelId, ct);
        return Ok(ApiResponse<IEnumerable<ReviewDto>>.Ok(result));
    }

    /// <summary>Submit a review for a hotel. [Authenticated]</summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewRequest request, CancellationToken ct)
    {
        var result = await reviewService.CreateAsync(request, CurrentUserId, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<ReviewDto>.Ok(result, "Review submitted successfully."));
    }
}