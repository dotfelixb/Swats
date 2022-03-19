using Microsoft.AspNetCore.Mvc.Rendering;
using SkiaSharp;
using SkiaSharp.QrCode;

namespace Swats.Extensions;

public static class MvcExtensions
{
    public static string ActiveClass(this IHtmlHelper htmlHelper
        , string controllers = null
        , string actions = null
        , string cssClass = "border-l-4 border-indigo-500")
    {
        var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
        var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

        var acceptedControllers = (controllers ?? currentController ?? "").Split(',');
        var acceptedActions = (actions ?? currentAction ?? "").Split(',');

        return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction)
            ? cssClass
            : "";
    }

    public static string GenerateQrCode(this string value)
    {
        using var generator = new QRCodeGenerator();
        // Generate QrCode
        var qr = generator.CreateQrCode(value, ECCLevel.L);

        // Render to canvas
        var info = new SKImageInfo(162, 162);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        canvas.Render(qr, info.Width, info.Height);

        // Output to Stream 
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = new MemoryStream();
        data.SaveTo(stream);
        return Convert.ToBase64String(stream.ToArray());
    }
}