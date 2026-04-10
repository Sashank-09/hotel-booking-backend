namespace HotelBooking.Application.DTOs.Loyalty;

public class LoyaltyTransactionDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid? BookingId { get; set; }
    public int Points { get; set; }
    public string Description { get; set; } = string.Empty;
    public int BalanceAfter { get; set; }
    public DateTime CreatedAt { get; set; }
}