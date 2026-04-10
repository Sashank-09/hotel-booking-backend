using HotelBooking.Domain.Enums;

namespace HotelBooking.Domain.Entities;

public class Room : BaseEntity
{
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<RoomAmenity> Amenities { get; set; } = new List<RoomAmenity>();
}