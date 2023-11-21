using ApplicationCommon.DTOs.Review;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(r => r.Review.BookingId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(r => r.Review.UserId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(r => r.Review.Ratings).NotEmpty();
        RuleFor(r => r.Review.Comment).NotEmpty().MaximumLength(500);
    }
}