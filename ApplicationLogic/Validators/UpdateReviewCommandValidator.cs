using ApplicationCommon.DTOs.Review;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(r => r.Review.Ratings).NotEmpty();
        RuleFor(r => r.Review.Comment).NotEmpty().MaximumLength(500);
    }
}