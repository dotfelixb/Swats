namespace Keis.Web.Extensions;

public static class HttpEx
{
    public static string UserId(this HttpContext context)
    {
        return context.User?.FindFirst("id")?.Value ?? "";
    }
}