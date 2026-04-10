using HotelBooking.Domain.Enums;

namespace HotelBooking.Application.DTOs.Booking;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType RoomType { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal OriginalPrice { get; set; }      // price before discount
    public decimal DiscountAmount { get; set; }     // how much was saved
    public decimal TotalPrice { get; set; }         // final price after discount
    public string? PromoCode { get; set; }          // which promo was applied
    public BookingStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string ConfirmationNumber { get; set; } = string.Empty;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
}