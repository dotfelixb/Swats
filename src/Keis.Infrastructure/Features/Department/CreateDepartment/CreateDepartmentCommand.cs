using FluentResults;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Manager { get; set; }
    public IEnumerable<SelectListItem> ManagerList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string BusinessHour { get; set; }
    public IEnumerable<SelectListItem> BusinessHours { get; set; } = Enumerable.Empty<SelectListItem>();
    public string OutgoingEmail { get; set; }
    public DefaultType Type { get; set; }
    public DefaultStatus Status { get; set; }
    public string Response { get; set; }
    public string CreatedBy { get; set; }
}