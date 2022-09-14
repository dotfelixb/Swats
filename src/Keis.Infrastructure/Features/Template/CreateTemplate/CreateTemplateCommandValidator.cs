using FluentValidation;

namespace Keis.Infrastructure.Features.Template.CreateTemplate;

public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Subject).NotEmpty();
        RuleFor(r => r.Body).NotEmpty().WithMessage("Template is required");
    }
}
