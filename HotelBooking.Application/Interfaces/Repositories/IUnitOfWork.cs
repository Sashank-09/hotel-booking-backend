using HotelBooking.Application.Interfaces.Repositories;

namespace HotelBooking.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IHotelRepository Hotels { get; }
    IRoomRepository Rooms { get; }
    IBookingRepository Bookings { get; }
    IReviewRepository Reviews { get; }
    IPromotionRepository Promotions { get; }
    ILoyaltyRepository LoyaltyTransactions { get; }
    IHotelOwnerRepository HotelOwners { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}