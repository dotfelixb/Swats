using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Manager { get; set; }
    public string BusinessHour { get; set; }
    public string OutgoingEmail { get; set; }
    public DefaultType Type { get; set; }
    public DefaultStatus Status { get; set; }
    public string Response { get; set; }
    public string CreatedBy { get; set; }
}