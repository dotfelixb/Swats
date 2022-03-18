using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateDepartmentCommand : IRequest<Result>
{
    public string Name { get; set; }
    public Guid Manager { get; set; }
    public Guid BusinessHour { get; set; }
    public string OutgoingEmail { get; set; }
    public string Response { get; set; }
    public DefaultType Type { get; set; }
}
