namespace AspireIssueTracker.Issues.UserEventListener;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(Worker)} is starting.");
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(1000, stoppingToken);
		}
		logger.LogInformation($"{nameof(Worker)} is stopping.");
	}
}