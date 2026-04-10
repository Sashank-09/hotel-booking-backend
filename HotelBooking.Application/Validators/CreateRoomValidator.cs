using FluentValidation;
using HotelBooking.Application.DTOs.Room;

namespace HotelBooking.Application.Validators;

public class CreateRoomValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel is required.");

        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Room number is required.")
            .MaximumLength(10);

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.MaxOccupancy)
            .InclusiveBetween(1, 10).WithMessage("Max occupancy must be between 1 and 10.");

        RuleFor(x => x.Description)
            .NotEmpty().MaximumLength(1000);
    }
}