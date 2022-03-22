using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Swats.ViewComponents;

public class BreadcrumbsViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(SelectListItem[] crumbs, string currentPage)
    {
        return await Task.Run(() => View(Tuple.Create(crumbs, currentPage)));
    }
}