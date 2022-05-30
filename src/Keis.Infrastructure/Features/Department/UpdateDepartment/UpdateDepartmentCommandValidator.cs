using FluentValidation;

namespace Keis.Infrastructure.Features.Department.UpdateDepartment;

public class UpdateDepartmentCommandValidator: AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Name).NotEmpty();
    }
}
