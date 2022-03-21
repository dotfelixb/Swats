using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class FetchTicket : Ticket
{
    public string DepartmentName { get; set; }
    public string RequesterName { get; set; }
    public string TeamName { get; set; }
    public string AssignedToName { get; set; }
    public string TicketTypeName { get; set; }
    public string HelpTopicName { get; set; }
}

public class FetchTicketType : TicketType
{
}
