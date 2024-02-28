using ApplicationLogic.Querying.Queries.ConversationQueries;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class GetConversationsQueryValidator : AbstractValidator<GetConversationsQuery>
{
    public GetConversationsQueryValidator()
    {
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.UserId).NotNull().When(x => x.Request.HostId is null);
        RuleFor(x => x.Request.HostId).NotNull().When(x => x.Request.UserId is null);
    }
}