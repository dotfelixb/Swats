using Microsoft.AspNetCore.Builder;

namespace Swats.Infrastructure.Extensions;

public static class SwatsExtension
{
    public static IApplicationBuilder UseSwatsSeed(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwatsSeedMiddleware>();
    }

    public static string FormatCode(this long value, string prefix = "", int width = 6, char padChar = '0')
            => $"{prefix}{value.ToString().PadLeft(width, padChar)}";
}