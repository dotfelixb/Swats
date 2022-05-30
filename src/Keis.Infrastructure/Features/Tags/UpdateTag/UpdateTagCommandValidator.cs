using FluentValidation;

namespace Keis.Infrastructure.Features.Tags.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Visibility).NotNull();
        RuleFor(c => c.Status).NotNull();
    }
}
