using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.ChangeTopic;

public class ChangeHelpTopicCommandValidator : AbstractValidator<ChangeHelpTopicCommand>
{
    public ChangeHelpTopicCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.HelpTopic).NotEmpty();
    }
}
