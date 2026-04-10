using AutoMapper;
using HotelBooking.Application.DTOs.Common;
using HotelBooking.Application.DTOs.Hotel;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class HotelService(IUnitOfWork uow, IMapper mapper) : IHotelService
{
    public async Task<PagedResult<HotelDto>> SearchAsync(SearchHotelRequest request, CancellationToken ct = default)
    {
        var result = await uow.Hotels.SearchAsync(request, ct);
        return new PagedResult<HotelDto>
        {
            Items = mapper.Map<IEnumerable<HotelDto>>(result.Items),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<HotelDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var hotel = await uow.Hotels.GetWithRoomsAsync(id, ct)
            ?? throw new NotFoundException(nameof(Hotel), id);
        return mapper.Map<HotelDto>(hotel);
    }

    public async Task<HotelDto> CreateAsync(CreateHotelRequest request, CancellationToken ct = default)
    {
        var hotel = mapper.Map<Hotel>(request);
        await uow.Hotels.AddAsync(hotel, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<HotelDto>(hotel);
    }

    public async Task<HotelDto> UpdateAsync(Guid id, CreateHotelRequest request, CancellationToken ct = default)
    {
        var hotel = await uow.Hotels.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Hotel), id);
        mapper.Map(request, hotel);
        uow.Hotels.Update(hotel);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<HotelDto>(hotel);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var hotel = await uow.Hotels.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Hotel), id);
        hotel.IsDeleted = true;
        uow.Hotels.Update(hotel);
        await uow.SaveChangesAsync(ct);
    }
}