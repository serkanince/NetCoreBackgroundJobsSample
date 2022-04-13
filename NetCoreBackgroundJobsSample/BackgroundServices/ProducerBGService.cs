using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreBackgroundJobsSample.Services;
using NetCoreBackgroundJobsSample.Services.Dto;
using NetCoreBackgroundJobsSample.Services.Enum;
using NetCoreBackgroundJobsSample.Services.Forex;
using NetCoreBackgroundJobsSample.Services.Providers.ExchangeRate;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.BackgroundServices
{
    public class ProducerBGService : IHostedService, IDisposable
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ProducerBGService> _logger;
        private readonly ConcurrentQueue<string> _queue;
        private CurrencyProviderContext _providerContext = null;
        private CurrencyCode PairOne, PairTwo;
        private int _executionCount = 0;
        private Timer _timer;

        public ProducerBGService(ILogger<ProducerBGService> logger, IHttpClientFactory clientFactory, ConcurrentQueue<string> queue)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _queue = queue;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service (Producer) running.");

            SetProviderSource();
            SetExchangePair();

            _timer = new Timer(GetExchangeAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void SetProviderSource()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("1 (TCMB - http://www.tcmb.gov.tr/kurlar)");
            Console.WriteLine("2 (Forex - https://www.freeforexapi.com/api)");
            Console.WriteLine("3 (Exchange Rate - https://api.exchangerate.host)");
            Console.WriteLine("Select Currency Rates provider :");

            string providerName = "Default provider (TCMB)";

            switch (Console.ReadLine())
            {
                case "1":
                    _providerContext = new CurrencyProviderContext(new TCMBApiProvider(_clientFactory));
                    providerName = "TCMB";
                    break;
                case "2":
                    _providerContext = new CurrencyProviderContext(new ForexApiProvider(_clientFactory));
                    providerName = "Forex";
                    break;
                case "3":
                    _providerContext = new CurrencyProviderContext(new ExchangeRateApiProvider(_clientFactory));
                    providerName = "Exchange Rate";
                    break;
                default:
                    _providerContext = new CurrencyProviderContext(new TCMBApiProvider(_clientFactory));
                    break;
            }


            Console.WriteLine(string.Format("Your selection provider is {0}", providerName));
        }

        private void SetExchangePair()
        {
            PairOne = CurrencyCode.USD;


            Console.WriteLine("--Base Currency is default USD !");

            Console.WriteLine("Please select Pairing Currency");
            Console.WriteLine("(Example : EUR,TRY)");

            do
            {
                Enum.TryParse<CurrencyCode>(Console.ReadLine(), out PairTwo);

            } while (PairTwo == CurrencyCode.NONE);


            Console.WriteLine("Loading...");
        }

        private async void GetExchangeAsync(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            GetCurrencyData currency = await _providerContext.GetCurrenctDataAsync(PairOne, PairTwo);

            if (currency != null)
            {
                var msg = string.Format("{3} Currency Data : Code : {0} , Rate : {1} ({2})", CurrencyCode.USD.ToString(), currency.Rate, currency.Date, count);
                _logger.LogInformation("Fetched Currency Data - - - " + count);
                _queue.Enqueue(msg);
            }
            else
            {
                _logger.LogWarning("Not Fetched Currency Data !! - - - " + count);
            }
        }

    }
}
