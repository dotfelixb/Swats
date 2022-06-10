using Microsoft.Extensions.Logging;
using Quartz;

namespace Keis.Infrastructure.Consumers.UserCreatedEmail;

public class UserCreatedEmailConsumer : IJob
{
    private readonly ILogger<UserCreatedEmailConsumer> _logger;

    public UserCreatedEmailConsumer(ILogger<UserCreatedEmailConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("User Created Email");
        await Task.CompletedTask;
    }
}
