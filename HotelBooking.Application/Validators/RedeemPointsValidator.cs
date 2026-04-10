using FluentValidation;
using HotelBooking.Application.DTOs.Loyalty;
namespace HotelBooking.Application.Validators;

public class RedeemPointsValidator : AbstractValidator<RedeemPointsRequest>
{
    public RedeemPointsValidator()
    {
        RuleFor(x => x.Points)
            .GreaterThan(0)
            .WithMessage("Points must be greater than zero.");

        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithMessage("Booking ID is required.");
    }
}