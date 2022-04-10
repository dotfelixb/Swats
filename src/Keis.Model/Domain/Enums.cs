namespace Keis.Model.Domain;

public enum DefaultType
{
    Public = 1,
    Private
}

public enum DefaultStatus
{
    Active = 1,
    Inactive
}

public enum TicketPriority
{
    Low = 1,
    Normal,
    High,
    Important
}

public enum TicketSource
{
    App = 1,
    Web,
    Call,
    Api
}

public enum CommentType
{
    Comment = 1,
    Reply,
    ReplyAll
}

public enum TicketStatus
{
    New = 1,
    Open, // Read Only except Requester

    Approved, // Ready for Assignee by Ticket Admin
    Assigned, // Is assigned

    Pending, // On hold, Only Assignee & Approval
    Review, // Same as Pending but can be comment on

    Close, // Only Approval User
    Deleted
}

public enum CommentSource
{
    Internal = 1,
    External
}

public enum AttachmentSource
{
    Ticket = 1,
    Comment
}

public enum AgentMode
{
    Agent = 1,
    User
}

public enum DefaultTimeFormat
{
    Minute = 1,
    Hour,
    Day
}