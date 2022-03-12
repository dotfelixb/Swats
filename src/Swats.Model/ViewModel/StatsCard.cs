namespace Swats.Model.ViewModel;

public class StatsCard
{
    public string? Title { get; init; }
    public int Count { get; init; }
    public string? ExtraClass { get; init; }
}

public class RecentTicket
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string[] Tags { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}


