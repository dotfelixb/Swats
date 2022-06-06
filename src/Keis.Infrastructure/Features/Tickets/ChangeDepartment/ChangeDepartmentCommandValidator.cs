using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.ChangeDepartment;

public class ChangeDepartmentCommandValidator : AbstractValidator<ChangeDepartmentCommand>
{
    public ChangeDepartmentCommandValidator()
    {
		RuleFor(r => r.Id).NotEmpty();
		RuleFor(r => r.Department).NotEmpty();
	}
}

