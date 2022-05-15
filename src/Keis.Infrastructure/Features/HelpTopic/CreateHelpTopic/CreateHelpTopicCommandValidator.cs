using FluentValidation;

namespace Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;

public class CreateHelpTopicCommandValidator : AbstractValidator<CreateHelpTopicCommand>
{
    public CreateHelpTopicCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Department).NotEmpty();
    }
}