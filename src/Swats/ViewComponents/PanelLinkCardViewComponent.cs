using Microsoft.AspNetCore.Mvc;
using Swats.Model.ViewModel;

namespace Swats.ViewComponents;

public class PanelLinkCardViewComponent : ViewComponent
{
    public PanelLinkCardViewComponent()
    {
    }

    public async Task<IViewComponentResult> InvokeAsync(string title, string iconName, string location = "/admin")
    {
        // Hook to Db
        return await Task.Run(() => View(new PanelLink { Title = title, IconName = iconName, Location = location}));
    }
}