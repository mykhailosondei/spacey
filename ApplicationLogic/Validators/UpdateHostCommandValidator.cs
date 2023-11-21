using ApplicationCommon.DTOs.Host;
using ApplicationLogic.Commanding.Commands.HostCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateHostCommandValidator : AbstractValidator<UpdateHostCommand>
{
    public UpdateHostCommandValidator()
    {
        RuleFor(r => r.Host.UserId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(r => r.Host.Rating).NotEmpty();
    }
}