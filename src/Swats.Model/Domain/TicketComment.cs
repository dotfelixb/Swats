using MassTransit;

namespace Swats.Model.Domain;

public class TicketComment : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public Guid Ticket { get; set; }
    public Guid Comment { get; set; }
    public string Title { get; set; }
    public string[] Receiptients { get; set; }
    public string Body { get; set; }
    public string CreatorName { get; set; }
    public string CreatorEmail { get; set; }
    public CommentSource Source { get; set; }
    public bool InternalOnly { get; set; }
}
