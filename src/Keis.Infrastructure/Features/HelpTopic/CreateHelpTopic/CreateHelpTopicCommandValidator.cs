using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;

public class CreateHelpTopicCommandValidator : AbstractValidator<CreateHelpTopicCommand>
{
    public CreateHelpTopicCommandValidator()
    {
        RuleFor(r => r.Topic).NotEmpty();
        RuleFor(r => r.Department).NotEmpty();
    }
}