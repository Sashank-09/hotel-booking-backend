using AutoMapper;
using HotelBooking.Application.DTOs.Review;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class ReviewService(IUnitOfWork uow, IMapper mapper) : IReviewService
{
    public async Task<IEnumerable<ReviewDto>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default)
    {
        var reviews = await uow.Reviews.GetByHotelIdAsync(hotelId, ct);
        return mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto> CreateAsync(CreateReviewRequest request, Guid userId, CancellationToken ct = default)
    {
        var hotelExists = await uow.Hotels.ExistsAsync(h => h.Id == request.HotelId, ct);
        if (!hotelExists)
            throw new NotFoundException(nameof(Hotel), request.HotelId);

        var review = mapper.Map<Review>(request);
        review.UserId = userId;

        await uow.Reviews.AddAsync(review, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ReviewDto>(review);
    }
}