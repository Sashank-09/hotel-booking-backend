using AutoMapper;
using HotelBooking.Application.DTOs.Loyalty;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class LoyaltyService(IUnitOfWork uow, IMapper mapper) : ILoyaltyService
{
    private const int PointsPerDollar = 10;

    public async Task<LoyaltySummaryDto> GetSummaryAsync(string userId, CancellationToken ct = default)
    {
        var transactions = await uow.LoyaltyTransactions.GetByUserIdAsync(userId, ct);
        var list = transactions.ToList();

        return new LoyaltySummaryDto
        {
            UserId = userId,
            TotalPoints = list.Any() ? list.First().BalanceAfter : 0,
            PointsEarned = list.Where(t => t.Points > 0).Sum(t => t.Points),
            PointsRedeemed = Math.Abs(list.Where(t => t.Points < 0).Sum(t => t.Points)),
            RecentTransactions = mapper.Map<List<LoyaltyTransactionDto>>(list.Take(10))
        };
    }

    public async Task EarnPointsAsync(string userId, Guid bookingId, decimal bookingAmount, CancellationToken ct = default)
    {
        var earned = (int)(bookingAmount * PointsPerDollar);
        var balance = await uow.LoyaltyTransactions.GetUserPointsBalanceAsync(userId, ct);

        var transaction = new LoyaltyTransaction
        {
            UserId = userId,
            BookingId = bookingId,
            Points = earned,
            Description = $"Earned from booking",
            BalanceAfter = balance + earned
        };

        await uow.LoyaltyTransactions.AddAsync(transaction, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task RedeemPointsAsync(string userId, int points, Guid bookingId, CancellationToken ct = default)
    {
        var balance = await uow.LoyaltyTransactions.GetUserPointsBalanceAsync(userId, ct);
        if (balance < points)
            throw new ValidationException("Insufficient loyalty points.");

        var transaction = new LoyaltyTransaction
        {
            UserId = userId,
            BookingId = bookingId,
            Points = -points,
            Description = "Redeemed for booking discount",
            BalanceAfter = balance - points
        };

        await uow.LoyaltyTransactions.AddAsync(transaction, ct);
        await uow.SaveChangesAsync(ct);
    }
}