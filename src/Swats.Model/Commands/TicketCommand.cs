using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateTicketCommand : IRequest<Result>
{
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string Body { get; set; }
    public string Assignee { get; set; }
    public string Agent { get; set; } // user by api to identify the swats user that created this ticket
    public TicketSource Source { get; set; }
    public string Type { get; set; }
    public IEnumerable<SelectListItem> TicketTypes { get; set; }
    public string Department { get; set; }
    public string HelpTopic { get; set; }
    public TicketPriority Priority { get; set; } = TicketPriority.Low;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public string CreatedBy { get; set; }
}

public class GetTicketTypeCommand : GetType, IRequest<Result<FetchTicketType>>
{
}

public class ListTicketTypeCommand : ListType, IRequest<Result<IEnumerable<FetchTicketType>>>
{
}

public class CreateTicketTypeCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}
