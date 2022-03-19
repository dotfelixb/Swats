using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateHelpTopicCommand : IRequest<Result>
{
    public string Topic { get; set; }
    public DefaultStatus Status { get; set; }
    public DefaultType Type { get; set; }
    public string LinkedDepartment { get; set; }
    public string DefaultDepartment { get; set; }
    public string Note { get; set; }
}
