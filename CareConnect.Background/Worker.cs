using Cronos;

namespace CareConnect.Background
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string schedule = "0 * * * *"; // every hour
        private readonly CronExpression _cron;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _cron = CronExpression.Parse(schedule);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                DateTime timeNow = DateTime.Now;
                var nextTime = _cron.GetNextOccurrence(timeNow);
                await Task.Delay(nextTime.Value - timeNow, stoppingToken);
            }
        }
    }
}