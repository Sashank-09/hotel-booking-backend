using HotelBooking.Domain.Enums;

namespace HotelBooking.Application.DTOs.Booking;

public class RebookDto
{
    public Guid NewBookingId { get; set; }
    public Guid OriginalBookingId { get; set; }
    public string ConfirmationNumber { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType RoomType { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}