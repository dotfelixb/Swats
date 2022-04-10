using Microsoft.AspNetCore.Mvc;

namespace Keis.ViewComponents;

public class AccountMenuViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Hook to Db
        return await Task.Run(() => View());
    }
}