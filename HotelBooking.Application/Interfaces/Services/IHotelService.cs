using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Hotel;

namespace HotelBooking.Application.Interfaces.Services;

public interface IHotelService
{
    Task<PagedResult<HotelDto>> SearchAsync(SearchHotelRequest request, CancellationToken ct = default);
    Task<HotelDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<HotelDto> CreateAsync(CreateHotelRequest request, CancellationToken ct = default);
    Task<HotelDto> UpdateAsync(Guid id, CreateHotelRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}