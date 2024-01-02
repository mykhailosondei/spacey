using ApplicationCommon.DTOs.User;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(50);
        RuleFor(r => r.PhoneNumber).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Address).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Description).NotEmpty().MaximumLength(500);
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
    }
}