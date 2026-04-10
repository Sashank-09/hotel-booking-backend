using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Infrastructure.Persistence;

namespace HotelBooking.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IHotelRepository Hotels { get; }
    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }
    public IReviewRepository Reviews { get; }
    public IPromotionRepository Promotions { get; }
    public ILoyaltyRepository LoyaltyTransactions { get; }
    public IHotelOwnerRepository HotelOwners { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Hotels = new HotelRepository(context);
        Rooms = new RoomRepository(context);
        Bookings = new BookingRepository(context);
        Reviews = new ReviewRepository(context);
        Promotions = new PromotionRepository(context);
        LoyaltyTransactions = new LoyaltyRepository(context);
        HotelOwners = new HotelOwnerRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}