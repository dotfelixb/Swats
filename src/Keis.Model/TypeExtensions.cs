namespace Keis.Model;

public static class TypeExtensions
{
    public static Guid ToGuid(this string value)
    {
        return Guid.TryParse(value, out var rst)
            ? rst
            : Guid.Empty;
    }
}