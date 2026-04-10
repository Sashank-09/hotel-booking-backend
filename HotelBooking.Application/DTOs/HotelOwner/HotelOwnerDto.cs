namespace HotelBooking.Application.DTOs.HotelOwner;

public class HotelOwnerDto
{
    public Guid Id { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}