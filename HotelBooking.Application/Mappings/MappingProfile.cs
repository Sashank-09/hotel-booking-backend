using AutoMapper;
using HotelBooking.Application.DTOs.Booking;
using HotelBooking.Application.DTOs.Hotel;
using HotelBooking.Application.DTOs.HotelOwner;
using HotelBooking.Application.DTOs.Loyalty;
using HotelBooking.Application.DTOs.Promotion;
using HotelBooking.Application.DTOs.Review;
using HotelBooking.Application.DTOs.Room;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hotel
        CreateMap<Hotel, HotelDto>();
        CreateMap<CreateHotelRequest, Hotel>();

        // Room
        CreateMap<Room, RoomDto>()
            .ForMember(d => d.HotelName,
                o => o.MapFrom(s => s.Hotel != null ? s.Hotel.Name : string.Empty))
            .ForMember(d => d.Amenities,
                o => o.MapFrom(s => s.Amenities.Select(a => a.Name).ToList()));
        CreateMap<CreateRoomRequest, Room>();

        // Booking
        CreateMap<Booking, BookingDto>()
    .ForMember(d => d.HotelName,
        o => o.MapFrom(s => s.Room != null && s.Room.Hotel != null ? s.Room.Hotel.Name : string.Empty))
    .ForMember(d => d.RoomNumber,
        o => o.MapFrom(s => s.Room != null ? s.Room.RoomNumber : string.Empty))
    .ForMember(d => d.RoomType,
        o => o.MapFrom(s => s.Room != null ? s.Room.Type : default))
    .ForMember(d => d.OriginalPrice, o => o.MapFrom(s => s.OriginalPrice))
    .ForMember(d => d.DiscountAmount, o => o.MapFrom(s => s.DiscountAmount))
    .ForMember(d => d.PromoCode, o => o.MapFrom(s => s.PromoCode));
        CreateMap<CreateBookingRequest, Booking>();

        // Review
        CreateMap<Review, ReviewDto>();
        CreateMap<CreateReviewRequest, Review>();

        // Promotion
        CreateMap<Promotion, PromotionDto>();
        CreateMap<CreatePromotionRequest, Promotion>();

        // Loyalty
        CreateMap<LoyaltyTransaction, LoyaltyTransactionDto>();

        // HotelOwner
        CreateMap<HotelOwnerProfile, HotelOwnerDto>();
        CreateMap<CreateHotelOwnerRequest, HotelOwnerProfile>();
    }
}