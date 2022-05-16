using FluentResults;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;

public class CreateHelpTopicCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public DefaultType Type { get; set; }
    public DefaultStatus Status { get; set; }
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string DefaultDepartment { get; set; }
    public string Note { get; set; }
    public string CreatedBy { get; set; }
}