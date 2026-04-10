namespace HotelBooking.Application.DTOs.HotelOwner;

public class CreateHotelOwnerRequest
{
    public string OwnerId { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? TaxId { get; set; }
}