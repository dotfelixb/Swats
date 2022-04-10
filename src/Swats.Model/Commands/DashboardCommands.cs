using FluentResults;
using MediatR;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class DashboardCommands { }

public class AgentTicketsCommand : GetType, IRequest<Result<int>>
{
    public bool OverdueOnly { get; set; }
    public bool DueToday { get; set; }
}

public class OpenTicketsCommand : IRequest<Result<int>>
{
    public bool OverdueOnly { get; set; }
}