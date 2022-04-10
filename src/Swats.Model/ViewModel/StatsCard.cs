namespace Swats.Model.ViewModel;

public class StatsCard
{
    public string Title { get; init; }
    public int Count { get; set; }
    public string ExtraClass { get; init; }
    public string Location { get; init; }
}

public class PanelLink
{
    public string Title { get; set; }
    public string IconName { get; set; }
    public string Location { get; set; }
}

public class IndexPartial
{
    public string Title { get; set; }
    public string CreateTitle { get; set; }
    public string CreateLocation { get; set; }
}