using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateAgentCommand : IRequest<Result<Guid>>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string Telephone { get; set; }
    public string Timezone { get; set; }
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Team { get; set; }
    public IEnumerable<SelectListItem> TeamList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Type { get; set; }
    public IEnumerable<SelectListItem> TypeList { get; set; } = Enumerable.Empty<SelectListItem>();
    public AgentMode Mode { get; set; }
    public Guid CreatedBy { get; set; }
}

public class GetAgentCommand : IRequest<Result<FetchedAgent>>
{
    public Guid Id { get; set; }
}

public class ListAgentCommand : IRequest<Result<IEnumerable<FetchedAgent>>>
{
    public int Offset { get; set; }
    public int Limit { get; set; }
    public bool Deleted { get; set; }
}