using Keis.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Keis.ViewComponents;

public class PanelLinkCardViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string title, string iconName, string location = "/admin")
    {
        // Hook to Db
        return await Task.Run(() => View(new PanelLink {Title = title, IconName = iconName, Location = location}));
    }
}