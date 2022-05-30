using FluentResults;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Teams.CreateTeam;

public class CreateTeamCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Department { get; set; }
    public string Manager { get; set; }
    public DefaultStatus Status { get; set; }
    public string Response { get; set; }
    public string CreatedBy { get; set; }
}