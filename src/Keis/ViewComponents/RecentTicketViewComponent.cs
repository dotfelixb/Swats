using Microsoft.AspNetCore.Mvc;
using Keis.Model.ViewModel;

namespace Keis.ViewComponents;

public class RecentTicketViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Hook to Db
        var recent = RecentTicketsList();

        return await Task.Run(() => View(recent));
    }

    private static IEnumerable<RecentTicket> RecentTicketsList()
    {
        return new List<RecentTicket>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Keis Application Down",
                Tags = new[] {"Urgent", "Application"},
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-12)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Schedule pending validation",
                Tags = new[] {"TA", "Application"},
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-10)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Payment matching issues",
                Tags = new[] {"TA", "Application", "Matching"},
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-8)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Tools Downtime",
                Tags = new[] {"Tools", "Application"},
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-8)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Deals sync issues",
                Tags = new[] {"TA", "CRM", "Application"},
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-5)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Portal Certification Error",
                Tags = new[] {"Members Portal", "CRM", "SSL"},
                CreatedAt = DateTimeOffset.UtcNow
            }
        };
    }
}