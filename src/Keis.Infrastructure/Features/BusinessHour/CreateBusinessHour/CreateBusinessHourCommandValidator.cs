using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.BusinessHour.CreateBusinessHour;

public class CreateBusinessHourCommandValidator : AbstractValidator<CreateBusinessHourCommand>
{
    public CreateBusinessHourCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
    }
}