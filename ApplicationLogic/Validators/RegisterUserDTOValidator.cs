using ApplicationCommon.DTOs.User;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
    }
}