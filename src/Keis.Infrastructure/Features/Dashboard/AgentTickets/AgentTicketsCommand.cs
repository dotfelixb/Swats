using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Dashboard.AgentTickets;

public class AgentTicketsCommand : GetType, IRequest<Result<int>>
{
    public bool OverdueOnly { get; set; }
    public bool DueToday { get; set; }
}