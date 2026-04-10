using FluentValidation;
using HotelBooking.Application.DTOs.Booking;

namespace HotelBooking.Application.Validators;

public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room is required.");

        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Check-in date must be today or a future date.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out must be after check-in.");

        RuleFor(x => x.NumberOfGuests)
            .InclusiveBetween(1, 10)
            .WithMessage("Guests must be between 1 and 10.");
    }
}