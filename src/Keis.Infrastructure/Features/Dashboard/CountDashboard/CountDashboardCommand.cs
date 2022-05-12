using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Dashboard.CountDashboard;

public class CountDashboardCommand : IRequest<Result<DashboardCount>>
{
    public string Id { get; set; }
}