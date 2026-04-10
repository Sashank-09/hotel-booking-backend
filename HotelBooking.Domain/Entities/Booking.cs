using HotelBooking.Domain.Enums;

namespace HotelBooking.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal OriginalPrice { get; set; }       // before promo
    public decimal DiscountAmount { get; set; }      // saving
    public decimal TotalPrice { get; set; }          // after promo
    public string? PromoCode { get; set; }           // promo applied
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? SpecialRequests { get; set; }
    public string ConfirmationNumber { get; set; } = GenerateConfirmation();

    // Navigation
    public Room Room { get; set; } = null!;

    private static string GenerateConfirmation() =>
        $"HB-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
}