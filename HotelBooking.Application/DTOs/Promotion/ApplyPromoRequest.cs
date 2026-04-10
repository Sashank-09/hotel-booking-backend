namespace HotelBooking.Application.DTOs.Promotion;

public class ApplyPromoRequest
{
    public string Code { get; set; } = string.Empty;
    public decimal BookingAmount { get; set; }
}