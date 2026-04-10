using HotelBooking.Application.DTOs.Review;

namespace HotelBooking.Application.Interfaces.Services;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default);
    Task<ReviewDto> CreateAsync(CreateReviewRequest request, Guid userId, CancellationToken ct = default);
}