using HotelBooking.Application.DTOs.Loyalty;

namespace HotelBooking.Application.Interfaces.Services;

public interface ILoyaltyService
{
    Task<LoyaltySummaryDto> GetSummaryAsync(string userId, CancellationToken ct = default);
    Task EarnPointsAsync(string userId, Guid bookingId, decimal bookingAmount, CancellationToken ct = default);
    Task RedeemPointsAsync(string userId, int points, Guid bookingId, CancellationToken ct = default);
}