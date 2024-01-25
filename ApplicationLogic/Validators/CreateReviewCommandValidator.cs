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
        RuleFor(r => r.Review.Ratings).NotEmpty().Must(ratings =>
             ratings.CheckIn is >= 0 and <= 5 &&
             ratings.Cleanliness is >= 0 and <= 5 &&
             ratings.Communication is >= 0 and <= 5 &&
             ratings.Location is >= 0 and <= 5 &&
             ratings.Accuracy is >= 0 and <= 5 &&
             ratings.Value is >= 0 and <= 5
        );
        RuleFor(r => r.Review.Comment).NotEmpty().MaximumLength(500);
    }
}