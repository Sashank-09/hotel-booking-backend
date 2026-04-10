namespace HotelBooking.Domain.Entities;

public class HotelOwnerProfile : BaseEntity
{
    public string OwnerId { get; set; } = string.Empty;   // links to AppUser identity Id
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public bool IsVerified { get; set; } = false;

    // Navigation
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}