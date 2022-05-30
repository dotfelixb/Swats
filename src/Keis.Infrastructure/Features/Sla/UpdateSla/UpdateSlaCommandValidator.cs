using FluentValidation;

namespace Keis.Infrastructure.Features.Sla.UpdateSla;

public class UpdateSlaCommandValidator : AbstractValidator<UpdateSlaCommand>
{
    public UpdateSlaCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.BusinessHour).NotEmpty();
        RuleFor(r => r.ResponsePeriod).GreaterThan(0);
        RuleFor(r => r.ResponsePeriod).GreaterThan(0);
    }
}
