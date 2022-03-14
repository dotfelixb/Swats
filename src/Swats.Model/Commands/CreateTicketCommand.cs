using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateTicketCommand
{
    public string Code { get; set; }
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string Body { get; set; }
    public Guid Assignee { get; set; }
    public string Agent { get; set; } // user by api to identify the swats user that created this ticket
    public Guid Source { get; set; }
    public Guid Type { get; set; }
    public Guid Department { get; set; }
    public Guid HelpTopic { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public Guid CreatedBy { get; set; }
}