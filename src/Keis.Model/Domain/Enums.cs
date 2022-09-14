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

public enum EventType
{
    NewTicket = 1,
    ChangeTicketType,
    ChangeDepartment,
    ChangeTeam,
    ChangeTicketPriority,
    ChangeTicketStatus
}

public enum WorkflowPriority
{
    Normal = 1,
    Medium,
    High,
    Important
}

public enum CriteriaType
{
    Subject = 1,
    Department,
    Team,
    Status,
    Priority
}

public enum ControlType
{
    Input = 1,
    Select,
    Multiselect
}

public enum CriteriaCondition
{
    Equals = 1,
    Contains
}

public enum ActionType
{
    ForwardTo = 1,
    AssignTo,
    AssignDepartment,
    AssignTeam,
    ApplySla,
    ChangeStatus
}

public enum EmailProtocol
{
    IMAP = 1,
    POP,
    SMTP,
    Exchange
}

public enum Encryption
{
    None = 1,
    SSL,
    TLS
}