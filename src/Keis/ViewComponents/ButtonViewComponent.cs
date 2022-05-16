using Keis.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Keis.ViewComponents;

public class ButtonViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string label, string buttontype)
    {
        // Hook to Db
        return await Task.Run(() => View(new KeisButton {Label = label, ButtonType = buttontype}));
    }
}