using Microsoft.AspNetCore.Builder;

namespace Keis.Infrastructure.Extensions;

public static class KeisExtension
{
    public static IApplicationBuilder UseKeisSeed(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<KeisSeedMiddleware>();
    }

    public static string FormatCode(this long value, string prefix = "", int width = 6, char padChar = '0')
    {
        return $"{prefix}{value.ToString().PadLeft(width, padChar)}";
    }
}