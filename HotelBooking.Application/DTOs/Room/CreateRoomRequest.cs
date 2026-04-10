using HotelBooking.Domain.Enums;

namespace HotelBooking.Application.DTOs.Room;

public class CreateRoomRequest
{
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = [];
}