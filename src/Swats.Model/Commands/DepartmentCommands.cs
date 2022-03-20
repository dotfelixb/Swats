using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

#region Department

public class CreateDepartmentCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Manager { get; set; }
    public IEnumerable<SelectListItem> ManagerList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string BusinessHour { get; set; }
    public IEnumerable<SelectListItem> BusinessHours { get; set; } = Enumerable.Empty<SelectListItem>();
    public string OutgoingEmail { get; set; }
    public DefaultType Type { get; set; }
    public string Response { get; set; }
    public string CreatedBy { get; set; }
}

public class GetDepartmentCommand : GetType, IRequest<Result<FetchDepartment>>
{
}

public class ListDepartmentCommand : ListType, IRequest<Result<IEnumerable<FetchDepartment>>>
{
}

#endregion

#region Team

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

public class GetTeamCommand : GetType, IRequest<Result<FetchTeam>>
{
}

public class ListTeamCommand : ListType, IRequest<Result<IEnumerable<FetchTeam>>>
{
}

#endregion