using FluentValidation;

namespace Keis.Infrastructure.Features.Sla.CreateSla;

public class CreateSlaCommandValidator : AbstractValidator<CreateSlaCommand>
{
    public CreateSlaCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.BusinessHour).NotEmpty();
        RuleFor(r => r.ResponsePeriod).GreaterThan(0);
        RuleFor(r => r.ResponsePeriod).GreaterThan(0);
    }
}