using Microsoft.AspNetCore.Mvc.Rendering;

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

    public static Guid ToGuid(this string value) => Guid.TryParse(value, out var rst) ? rst : Guid.Empty;
}