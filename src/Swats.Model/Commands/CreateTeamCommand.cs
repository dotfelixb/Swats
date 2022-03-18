using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateTeamCommand : IRequest<Result>
{
    public string Name { get; set; }
    public Guid Lead { get; set; }
    public Guid Department { get; set; }
    public DefaultStatus Status { get; set; }
    public string Notes { get; set; }
}
