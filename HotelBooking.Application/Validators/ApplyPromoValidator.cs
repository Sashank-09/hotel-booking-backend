using FluentValidation;
using HotelBooking.Application.DTOs.Promotion;

namespace HotelBooking.Application.Validators;

public class ApplyPromoValidator : AbstractValidator<ApplyPromoRequest>
{
    public ApplyPromoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Promo code is required.")
            .MaximumLength(50).WithMessage("Promo code cannot exceed 50 characters.");

        RuleFor(x => x.BookingAmount)
            .GreaterThan(0).WithMessage("Booking amount must be greater than zero.");
    }
}