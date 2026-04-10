using AutoMapper;
using HotelBooking.Application.DTOs.HotelOwner;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Infrastructure.Services;

public class HotelOwnerService(IUnitOfWork uow, IMapper mapper) : IHotelOwnerService
{
    public async Task<HotelOwnerDto> CreateProfileAsync(CreateHotelOwnerRequest request, CancellationToken ct = default)
    {
        var existing = await uow.HotelOwners.GetByOwnerIdAsync(request.OwnerId, ct);
        if (existing != null)
            throw new ConflictException("Owner profile already exists.");

        var profile = mapper.Map<HotelOwnerProfile>(request);
        await uow.HotelOwners.AddAsync(profile, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<HotelOwnerDto>(profile);
    }

    public async Task<HotelOwnerDto> GetByOwnerIdAsync(string ownerId, CancellationToken ct = default)
    {
        var profile = await uow.HotelOwners.GetByOwnerIdAsync(ownerId, ct)
            ?? throw new NotFoundException(nameof(HotelOwnerProfile), ownerId);
        return mapper.Map<HotelOwnerDto>(profile);
    }

    public async Task<HotelOwnerDto> VerifyOwnerAsync(string ownerId, CancellationToken ct = default)
    {
        var profile = await uow.HotelOwners.GetByOwnerIdAsync(ownerId, ct)
            ?? throw new NotFoundException(nameof(HotelOwnerProfile), ownerId);
        profile.IsVerified = true;
        uow.HotelOwners.Update(profile);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<HotelOwnerDto>(profile);
    }
}