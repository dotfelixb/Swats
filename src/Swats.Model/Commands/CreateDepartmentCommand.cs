using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateDepartmentCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Manager { get; set; }
    public string BusinessHour { get; set; }
    public IEnumerable<SelectListItem> BusinessHours { get; set; }
    public string OutgoingEmail { get; set; }
    public string Response { get; set; }
    public DefaultType Type { get; set; }
}
