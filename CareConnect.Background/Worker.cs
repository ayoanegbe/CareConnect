using Cronos;

namespace CareConnect.Background
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string schedule = "0 * * * *"; // every hour
        private readonly CronExpression _cron;
        private int _executionCount;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _cron = CronExpression.Parse(schedule);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ++_executionCount;
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //DateTime timeNow = DateTime.Now;
                //var nextTime = _cron.GetNextOccurrence(timeNow);
                //await Task.Delay(nextTime.Value - timeNow, stoppingToken);
                _logger.LogInformation(
                "{ServiceName} working, execution count: {Count}",
                "Worket",
                _executionCount);

                await Task.Delay(10_000, stoppingToken);
            }
        }
    }
}