using FluentValidation;
using HotelBooking.Application.DTOs.Promotion;

namespace HotelBooking.Application.Validators;

public class CreatePromotionValidator : AbstractValidator<CreatePromotionRequest>
{
    public CreatePromotionValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.DiscountValue).GreaterThan(0);
        RuleFor(x => x.ValidFrom).LessThan(x => x.ValidTo)
            .WithMessage("Valid From must be before Valid To.");
        RuleFor(x => x.ValidTo).GreaterThan(DateTime.UtcNow)
            .WithMessage("Valid To must be a future date.");
    }
}