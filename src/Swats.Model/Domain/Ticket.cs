namespace Swats.Model.Domain;

public class Ticket : DbAudit
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string CreatorName { get; set; }
    public string CreatorEmail { get; set; }
    public Guid AssignedTo { get; set; }
    public Guid Source { get; set; }
    public Guid Department { get; set; }
    public TicketPriority Priority { get; set; }
    public DateOnly DueDate { get; set; }
    public TicketStatus Status { get; set; }
    public bool Reopened { get; set; }
}

public class Tag : DbAudit
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}

public class TicketTag : DbAudit
{
    public Guid Id { get; set; }
    public Guid Ticket { get; set; }
    public Guid Tag { get; set; }
}

public class TicketFellow : DbAudit
{
    public Guid Id { get; set; }
    public Guid Ticket { get; set; }
    public Guid Fellow { get; set; }
    public int Points { get; set; }
    public bool UnFollow { get; set; }
}

public class Department : DbAudit
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}

public class Sla : DbAudit
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Hours { get; set; }
}

public class TicketSla : DbAudit
{
    public Guid Id { get; set; }
    public Guid Ticket { get; set; }
    public Guid Sla { get; set; }
}

public class TicketSource : DbAudit
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}

public class TicketComment : DbAudit
{
    public Guid Id { get; set; }
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
    public Guid Id { get; set; }
    public Guid Target { get; set; }
    public AttachmentSource Source { get; set; }
    public string FilePath { get; set; }
    public string AbsolutePath { get; set; }
}

public class User : DbAudit
{

}

public enum TicketPriority
{
    Low,
    Normal,
    High,
    Important
}

public enum TicketStatus
{
    Open, // Read Only except Requester

    Approved, // Ready for Assignee by Ticket Admin
    Assigned, // Is assigned

    Pending, // On hold, Only Assignee & Approval
    Review, // Same as Pending but can be comment on

    Close, // Only Approval User
    Deleted,
}

public enum CommentSource
{
    Internal,
    External
}

public enum AttachmentSource
{
    Ticket,
    Comment
}