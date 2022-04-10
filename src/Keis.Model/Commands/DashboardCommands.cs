using FluentResults;
using MediatR;
using Keis.Model.Queries;

namespace Keis.Model.Commands;

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