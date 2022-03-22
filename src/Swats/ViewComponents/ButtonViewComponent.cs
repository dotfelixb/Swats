using Microsoft.AspNetCore.Mvc;
using Swats.Model.ViewModel;

namespace Swats.ViewComponents;

public class ButtonViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string label, string buttontype)
    {
        // Hook to Db
        return await Task.Run(() => View(new SwatsButton {Label = label, ButtonType = buttontype}));
    }
}