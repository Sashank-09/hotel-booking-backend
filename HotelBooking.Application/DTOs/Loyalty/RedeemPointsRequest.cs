namespace HotelBooking.Application.DTOs.Loyalty;

public class RedeemPointsRequest
{
    public int Points { get; set; }
    public Guid BookingId { get; set; }
}