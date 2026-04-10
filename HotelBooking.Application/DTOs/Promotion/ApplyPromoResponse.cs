namespace HotelBooking.Application.DTOs.Promotion;

public class ApplyPromoResponse
{
    public bool IsValid { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string Message { get; set; } = string.Empty;
}