using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.BackgroundServices
{
    public class ConsumerBGService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ConcurrentQueue<string> _queue;

        public ConsumerBGService(ILogger<ConsumerBGService> logger, ConcurrentQueue<string> queue)
        {
            _logger = logger;
            _queue = queue;
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service (Consumer) running.");


            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                if (_queue.TryDequeue(out string message))
                {
                    _logger.LogInformation($"Got a new message: {message}. (Queue size: {_queue.Count})");
                }
            }
        }


    }
}
