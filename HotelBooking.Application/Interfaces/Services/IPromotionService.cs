using HotelBooking.Application.DTOs.Promotion;

namespace HotelBooking.Application.Interfaces.Services;

public interface IPromotionService
{
    Task<PromotionDto> CreateAsync(CreatePromotionRequest request, CancellationToken ct = default);
    Task<IEnumerable<PromotionDto>> GetActiveAsync(CancellationToken ct = default);
    Task<ApplyPromoResponse> ApplyAsync(ApplyPromoRequest request, CancellationToken ct = default);
    Task<IEnumerable<PromotionDto>> GetByHotelIdAsync(Guid hotelId, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
}