using FluentResults;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Teams.CreateTeam;

public class CreateTeamCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Lead { get; set; }
    public IEnumerable<SelectListItem> LeadList { get; set; } = Enumerable.Empty<SelectListItem>();
    public DefaultStatus Status { get; set; }
    public string Note { get; set; }
    public string CreatedBy { get; set; }
}