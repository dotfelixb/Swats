using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.Tags.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Visibility).NotNull();
        RuleFor(c => c.Status).NotNull();
    }
}