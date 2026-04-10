using HotelBooking.Application.DTOs.Booking;
using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Create a new booking for the authenticated user.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request, CancellationToken ct)
    {
        var result = await bookingService.CreateAsync(request, CurrentUserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<BookingDto>.Ok(result, "Booking confirmed successfully."));
    }

    /// <summary>Get a booking by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await bookingService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<BookingDto>.Ok(result));
    }

    /// <summary>Get all bookings for the authenticated user (paged).</summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyBookings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await bookingService.GetUserBookingsAsync(CurrentUserId, page, pageSize, ct);
        return Ok(ApiResponse<PagedResult<BookingDto>>.Ok(result));
    }

    /// <summary>Cancel a booking. Only the owner can cancel.</summary>
    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var result = await bookingService.CancelAsync(id, CurrentUserId, ct);
        return Ok(ApiResponse<BookingDto>.Ok(result, "Booking cancelled successfully."));
    }

    /// <summary>Rebook from an existing booking with new dates.</summary>
    [HttpPost("{id:guid}/rebook")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Rebook(
        Guid id, [FromBody] RebookRequest request, CancellationToken ct)
    {
        // Map RebookRequest → CreateBookingRequest (RoomId left empty = reuse original)
        var createRequest = new CreateBookingRequest
        {
            RoomId = Guid.Empty,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            NumberOfGuests = request.NumberOfGuests,
            SpecialRequests = request.SpecialRequests,
            PromoCode = request.PromoCode
        };

        var result = await bookingService.RebookAsync(id, CurrentUserId, createRequest, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<BookingDto>.Ok(result, "Rebooking confirmed successfully."));
    }
}