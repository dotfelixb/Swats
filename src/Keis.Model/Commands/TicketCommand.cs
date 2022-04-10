using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Keis.Model.Domain;
using Keis.Model.Queries;

namespace Keis.Model.Commands;

#region Ticket

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
    public TicketSource Source { get; set; }
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

public class GetTicketCommand : GetType, IRequest<Result<FetchTicket>>
{
}

public class AssignTicketCommand : IRequest<Result<SingleResult<string>>>
{
    public string Id { get; set; }
    public string AssignedTo { get; set; }
    public string CreatedBy { get; set; }
}

public class AssignTicketDepartmentCommand : IRequest<Result<SingleResult<string>>>
{
    public string Id { get; set; }
    public string Department { get; set; }
    public string CreatedBy { get; set; }
}

public class AssignTicketTeamCommand : IRequest<Result<SingleResult<string>>>
{
    public string Id { get; set; }
    public string Team { get; set; }
    public string CreatedBy { get; set; }
}

public class ListTicketCommand : ListType, IRequest<Result<ListResult<FetchTicket>>>
{
    public string Id { get; set; }
    public bool IncludeDepartment { get; set; }
    public bool IncludeTeam { get; set; }
    public bool IncludeHelpTopic { get; set; }
}

public class TicketCommentCommand
{
    public string CommentId { get; set; }
    public string TicketId { get; set; }
    public string  Body { get; set; }
}

public class CreateTicketCommentCommand : TicketCommentCommand, IRequest<Result<string>>
{
    public string FromEmail { get; set; }
    public string FromName { get; set; }    
    public string CreatedBy { get; set; }
}

public class CreateTicketReplyCommand :TicketCommentCommand, IRequest<Result<string>>
{
    public string Subject { get; set; }
    public string ToEmail { get; set; }
    public string[][] Recipients { get; set; }
}


public class ListTicketCommentCommand : ListType, IRequest<Result<IEnumerable<FetchTicketComment>>>
{
    public string TicketId { get; set; }
}

#endregion

#region Ticket Type

public class CreateTicketTypeCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

public class GetTicketTypeCommand : GetType, IRequest<Result<FetchTicketType>>
{
}

public class ListTicketTypeCommand : ListType, IRequest<Result<IEnumerable<FetchTicketType>>>
{
}

#endregion
