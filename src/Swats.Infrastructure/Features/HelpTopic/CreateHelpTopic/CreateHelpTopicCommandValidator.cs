using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.HelpTopic.CreateHelpTopic;

public class CreateHelpTopicCommandValidator : AbstractValidator<CreateHelpTopicCommand>
{
    public CreateHelpTopicCommandValidator()
    {
        RuleFor(r => r.Topic).NotEmpty();
        RuleFor(r => r.Department).NotEmpty();
    }
}