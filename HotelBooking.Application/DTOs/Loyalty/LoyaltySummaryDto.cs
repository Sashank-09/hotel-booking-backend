namespace HotelBooking.Application.DTOs.Loyalty;

public class LoyaltySummaryDto
{
    public string UserId { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
    public int PointsEarned { get; set; }
    public int PointsRedeemed { get; set; }
    public List<LoyaltyTransactionDto> RecentTransactions { get; set; } = [];
}