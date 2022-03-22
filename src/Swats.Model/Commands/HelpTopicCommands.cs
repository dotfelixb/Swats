using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateHelpTopicCommand : IRequest<Result<string>>
{
    public string Topic { get; set; }
    public DefaultType Type { get; set; }
    public DefaultStatus Status { get; set; }
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string DefaultDepartment { get; set; }
    public string Note { get; set; }
    public string CreatedBy { get; set; }
}

public class GetHelpTopicCommand : GetType, IRequest<Result<FetchHelpTopic>>
{
}

public class ListHelpTopicsCommand : ListType, IRequest<Result<IEnumerable<FetchHelpTopic>>>
{
}