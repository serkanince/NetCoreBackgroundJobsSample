using NetCoreBackgroundJobsSample.Services.Dto;
using NetCoreBackgroundJobsSample.Services.Enum;
using NetCoreBackgroundJobsSample.Services.Forex.Dto;
using NetCoreBackgroundJobsSample.Services.Providers;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.Services.Forex
{
    public class ForexApiProvider : BaseClient, ICurrencyProvider
    {
        private const string _freeforexapiUrl = "https://www.freeforexapi.com";


        public ForexApiProvider(IHttpClientFactory clientFactory) : base(clientFactory, new Uri(_freeforexapiUrl))
        {
        }

        public async Task<GetCurrencyData> GetCurrencyDataAsync(CurrencyCode currencyCodePairOne, CurrencyCode currencyCodePairTwo)
        {
            var apiAction = "api/live?pairs=" + currencyCodePairOne.ToString() + currencyCodePairTwo.ToString();

            var response = await GetAsync<ForexCurrencyOutput>(apiAction);

            return response.rates == null ? null : new GetCurrencyData()
            {
                Rate = response.rates.USDTRY.rate.ToString(),
                Date = UnixTimeStampToDateTime(response.rates.USDTRY.timestamp)
            };
        }

        /// <summary>
        /// Ref : https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        /// </summary>
        /// <param name="unixTimeStamp">Unix Time Stamp</param>
        /// <returns></returns>
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
