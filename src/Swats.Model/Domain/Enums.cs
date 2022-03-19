namespace Swats.Model.Domain;

public enum DefaultType
{
    Public,
    Private
}

public enum DefaultStatus
{
    Active,
    Inactive
}

public enum TicketPriority
{
    Low,
    Normal,
    High,
    Important
}

public enum TicketSource
{
    App,
    Web,
    Call,
    Api,
}

public enum TicketStatus
{
    New,
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

public enum AgentMode
{
    Agent,
    User
}