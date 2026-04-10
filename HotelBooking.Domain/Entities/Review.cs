namespace HotelBooking.Domain.Entities;

public class Review : BaseEntity
{
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }          // 1–5
    public string Comment { get; set; } = string.Empty;

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;
}