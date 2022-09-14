using Quartz;

namespace Keis.Infrastructure.Consumers.ReadEmail;

public class SupportEmailConsumer : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        return Task.CompletedTask;
    }
}
