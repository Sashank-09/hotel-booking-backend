namespace HotelBooking.Domain.Entities;

public class RoomAmenity : BaseEntity
{
    public Guid RoomId { get; set; }
    public string Name { get; set; } = string.Empty;      // e.g. "WiFi", "Air Conditioning"
    public string? Icon { get; set; }                      // optional icon identifier

    // Navigation
    public Room Room { get; set; } = null!;
}