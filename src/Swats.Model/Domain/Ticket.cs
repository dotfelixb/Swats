using MassTransit;

namespace Swats.Model.Domain;

public class Ticket : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string Body { get; set; }
    public Guid Assignee { get; set; }
    public Guid Source { get; set; }
    public Guid Type { get; set; }
    public Guid Department { get; set; }
    public Guid HelpTopic { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }

}

public class TicketType : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
}

public class TicketTypeAuditLog : DbAuditLog { }

public class Tag : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public string Name { get; set; }
}

public class TicketTag : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Tag { get; set; }
}

public class TicketFellow : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Fellow { get; set; }
    public int Points { get; set; }
    public bool UnFollow { get; set; }
}

public class Department : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public string Name { get; set; }
}

public class Sla : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Hours { get; set; }
}

public class TicketSla : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Sla { get; set; }
}

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

public class Attachment : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Target { get; set; }
    public AttachmentSource Source { get; set; }
    public string FilePath { get; set; }
    public string AbsolutePath { get; set; }
}

public class Agent : DbAudit
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Telephone { get; set; }
    public Guid Timezone { get; set; }
    public Guid Department { get; set; }
    public Guid Team { get; set; }
}