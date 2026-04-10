namespace HotelBooking.Domain.Entities;

public class LoyaltyTransaction : BaseEntity
{
    public string UserId { get; set; } = string.Empty;    // links to AppUser identity Id
    public Guid? BookingId { get; set; }
    public int Points { get; set; }                        // positive = earned, negative = redeemed
    public string Description { get; set; } = string.Empty; // e.g. "Earned from booking HB-20260409-XXXX"
    public int BalanceAfter { get; set; }                  // snapshot of balance after this transaction

    // Navigation
    public Booking? Booking { get; set; }
}