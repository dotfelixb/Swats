namespace Swats.Model.ViewModel;

public class RecentTicket
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string[] Tags { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}