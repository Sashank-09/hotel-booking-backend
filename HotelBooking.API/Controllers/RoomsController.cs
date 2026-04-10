using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Room;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    /// <summary>Get all rooms for a hotel.</summary>
    [HttpGet("hotel/{hotelId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoomDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHotel(Guid hotelId, CancellationToken ct)
    {
        var result = await roomService.GetByHotelIdAsync(hotelId, ct);
        return Ok(ApiResponse<IEnumerable<RoomDto>>.Ok(result));
    }

    /// <summary>Get a room by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await roomService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<RoomDto>.Ok(result));
    }

    /// <summary>Create a new room. [Admin, HotelManager]</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,HotelManager")]
    [ProducesResponseType(typeof(ApiResponse<RoomDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoomRequest request, CancellationToken ct)
    {
        var result = await roomService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<RoomDto>.Ok(result, "Room created successfully."));
    }

    /// <summary>Update a room. [Admin, HotelManager]</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,HotelManager")]
    [ProducesResponseType(typeof(ApiResponse<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] CreateRoomRequest request, CancellationToken ct)
    {
        var result = await roomService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse<RoomDto>.Ok(result, "Room updated successfully."));
    }

    /// <summary>Soft-delete a room. [Admin, HotelManager]</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,HotelManager")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await roomService.DeleteAsync(id, ct);
        return Ok(ApiResponse<object>.Ok(null!, "Room deleted successfully."));
    }
}