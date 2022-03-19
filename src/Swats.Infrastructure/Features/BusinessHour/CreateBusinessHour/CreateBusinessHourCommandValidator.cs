using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.BusinessHour.CreateBusinessHour;

public class CreateBusinessHourCommandValidator : AbstractValidator<CreateBusinessHourCommand>
{
    public CreateBusinessHourCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
    }
}

