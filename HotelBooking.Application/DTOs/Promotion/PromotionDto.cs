using HotelBooking.Domain.Enums;

namespace HotelBooking.Application.DTOs.Promotion;

public class PromotionDto
{
    public Guid Id { get; set; }
    public Guid? HotelId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PromoType Type { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinimumBookingAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }
}