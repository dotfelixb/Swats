using Microsoft.AspNetCore.Mvc;
using Swats.Model.ViewModel;

namespace Swats.ViewComponents;

public class RecentTicketViewComponent : ViewComponent
{
    public RecentTicketViewComponent()
    {

    }


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
            new RecentTicket {
                Id = Guid.NewGuid(),
                Title = "Swats Application Down",
                Tags = new []{ "Urgent", "Application" },
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-12)
            },
             new RecentTicket {
                Id = Guid.NewGuid(),
                Title = "Schedule pending validation",
                Tags = new []{ "TA", "Application" },
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-10)
            },
            new RecentTicket {
                Id = Guid.NewGuid(),
                Title = "Payment matching issues",
                Tags = new []{ "TA", "Application", "Matching" },
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-8)
            },
             new RecentTicket {
                Id = Guid.NewGuid(),
                Title = "Tools Downtime",
                Tags = new []{ "Tools", "Application",  },
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-8)
            },
             new RecentTicket {
                Id = Guid.NewGuid(),
                Title = "Deals sync issues",
                Tags = new []{ "TA", "CRM", "Application" },
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-5)
            },
             new RecentTicket {
                Id = Guid.NewGuid(),
                Title = "Portal Certification Error",
                Tags = new []{ "Members Portal", "CRM", "SSL" },
                CreatedAt = DateTimeOffset.UtcNow
            },
        };
    }
}

