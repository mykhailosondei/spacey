using ApplicationCommon.DTOs.User;
using ApplicationLogic.Commanding.Commands.UserCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(r => r.User.Name).NotEmpty().MaximumLength(50);
        RuleFor(r => r.User.PhoneNumber).MaximumLength(50);
        RuleFor(r => r.User.Address).NotNull();
        RuleFor(r => r.User.Description).MaximumLength(500);
    }
}