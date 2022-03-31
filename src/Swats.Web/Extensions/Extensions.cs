namespace Swats.Web.Extensions;

public static class Extensions
{
    public static string GetUserId(this HttpContext context) 
        => context.User?.FindFirst("id")?.Value ?? "";
}