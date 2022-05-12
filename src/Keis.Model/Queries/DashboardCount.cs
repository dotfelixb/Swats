namespace Keis.Model.Queries;

public class DashboardCount
{
    public int MyTicket { get; set; }
    public int MyOverDue { get; set; }
    public int MyDueToday { get; set; }
    public int OpenTickets { get; set; }
    public int OpenTicketsDue { get; set; }
}