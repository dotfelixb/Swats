using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
    }
}