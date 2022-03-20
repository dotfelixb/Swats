namespace Swats.Model.Queries;

public class GetType
{
    public string Id { get; set; }
}

public class ListType
{
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 20;
    public bool Deleted { get; set; }
}
