namespace Keis.Model.Queries;

public class ListType
{
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 1000;
    public bool Deleted { get; set; }
}