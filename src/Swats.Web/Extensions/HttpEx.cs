namespace Swats.Web.Extensions;

public static class HttpEx
{
    public static string UserId(this HttpContext context) 
        => context.User?.FindFirst("id")?.Value ?? "";
}