using FluentResults;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketCommand : IRequest<Result<string>>
{
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string RequesterEmail { get; set; }
    public string RequesterName { get; set; }
    public IEnumerable<SelectListItem> RequesterList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Body { get; set; }
    public string AssignedTo { get; set; }
    public IEnumerable<SelectListItem> AssigneeList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string ExternalAgent { get; set; } // user by api to identify the keis user that created this ticket
    public TicketSource Source { get; set; } = TicketSource.App;
    public string TicketType { get; set; }
    public IEnumerable<SelectListItem> TypeList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Team { get; set; }
    public IEnumerable<SelectListItem> TeamList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string HelpTopic { get; set; }
    public IEnumerable<SelectListItem> HelpTopicList { get; set; } = Enumerable.Empty<SelectListItem>();
    public TicketPriority Priority { get; set; } = TicketPriority.Low;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public string CreatedBy { get; set; }
}