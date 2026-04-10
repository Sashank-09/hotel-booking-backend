namespace HotelBooking.Application.DTOs.Email;

public class CancellationEmailDto
{
    public string ToEmail { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string ConfirmationNumber { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}