using HotelBooking.Domain.Enums;

namespace HotelBooking.Domain.Entities;

public class Promotion : BaseEntity
{
    public Guid? HotelId { get; set; }                    // null = platform-wide promo
    public string Code { get; set; } = string.Empty;      // e.g. "SUMMER20"
    public string Description { get; set; } = string.Empty;
    public PromoType Type { get; set; }
    public decimal DiscountValue { get; set; }             // % or flat amount
    public decimal? MinimumBookingAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; } = 0;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Hotel? Hotel { get; set; }
}