using FluentResults;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Keis.Model.Domain;
using Keis.Model.Queries;

namespace Keis.Model.Commands;







public class AssignAgentDepartmentCommand : IRequest<Result<SingleResult<string>>>
{
    public string Id { get; set; }
    public string Department { get; set; }
    public string CreatedBy { get; set; }
}

public class AssignAgentTeamCommand : IRequest<Result<SingleResult<string>>>
{
    public string Id { get; set; }
    public string Team { get; set; }
    public string CreatedBy { get; set; }
}