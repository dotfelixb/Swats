using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Tags.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Visibility).NotNull();
        RuleFor(c => c.Status).NotNull();
    }
}