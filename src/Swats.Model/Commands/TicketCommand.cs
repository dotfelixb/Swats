using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateTicketCommand : IRequest<Result>
{
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string Body { get; set; }
    public string Assignee { get; set; }
    public string Agent { get; set; } // user by api to identify the swats user that created this ticket
    public string Source { get; set; }
    public string Type { get; set; }
    public string Department { get; set; }
    public string HelpTopic { get; set; }
    public TicketPriority Priority { get; set; } = TicketPriority.Low;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public Guid CreatedBy { get; set; }
}
