using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
    }
}