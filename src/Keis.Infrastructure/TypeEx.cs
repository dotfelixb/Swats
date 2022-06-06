using Keis.Model.Domain;

namespace Keis.Infrastructure;

public static class TypeEx
{
    public static string ToText(this TicketStatus value)
    {
        return value switch
        {
            TicketStatus.New => "New",
            TicketStatus.Open => "Open",
            TicketStatus.Approved => "Approved",
            TicketStatus.Assigned => "Assigned",
            TicketStatus.Pending => "Pending",
            TicketStatus.Review => "Review",
            TicketStatus.Close => "Close",
            TicketStatus.Deleted => "Deleted",
            _ => "Unassigned"
        };
    }

    public static string ToText(this TicketPriority value)
    {
        return value switch
        {
            TicketPriority.Low => "Low",
            TicketPriority.Normal=>"Normal",
            TicketPriority.High=> "High",
            TicketPriority.Important=>"Important!",
            _ => "Unassigned"
        };
    }
}