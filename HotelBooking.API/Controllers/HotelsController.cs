using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Hotel;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(IHotelService hotelService) : ControllerBase
{
    /// <summary>Search hotels with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<HotelDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchHotelRequest request, CancellationToken ct)
    {
        var result = await hotelService.SearchAsync(request, ct);
        return Ok(ApiResponse<PagedResult<HotelDto>>.Ok(result));
    }

    /// <summary>Get a hotel by ID including its rooms.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<HotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await hotelService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<HotelDto>.Ok(result));
    }

    /// <summary>Create a new hotel. [Admin only]</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<HotelDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateHotelRequest request, CancellationToken ct)
    {
        var result = await hotelService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<HotelDto>.Ok(result, "Hotel created successfully."));
    }

    /// <summary>Update a hotel. [Admin only]</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<HotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] CreateHotelRequest request, CancellationToken ct)
    {
        var result = await hotelService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse<HotelDto>.Ok(result, "Hotel updated successfully."));
    }

    /// <summary>Soft-delete a hotel. [Admin only]</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await hotelService.DeleteAsync(id, ct);
        return Ok(ApiResponse<object>.Ok(null!, "Hotel deleted successfully."));
    }
}