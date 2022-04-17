using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Keis.Model.Domain;
using Keis.Model.Queries;

namespace Keis.Model.Commands;

#region Ticket

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

public class TicketCommentCommand
{
    public string CommentId { get; set; }
    public string TicketId { get; set; }
    public string  Body { get; set; }
}

public class CreateTicketReplyCommand :TicketCommentCommand, IRequest<Result<string>>
{
    public string Subject { get; set; }
    public string ToEmail { get; set; }
    public string[][] Recipients { get; set; }
}

#endregion

