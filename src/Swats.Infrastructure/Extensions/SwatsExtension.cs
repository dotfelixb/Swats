using Microsoft.AspNetCore.Builder;

namespace Swats.Infrastructure.Extensions;

public static class SwatsExtension
{
    public static IApplicationBuilder UseSwatsSeed(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwatsSeedMiddleware>();
    }
}