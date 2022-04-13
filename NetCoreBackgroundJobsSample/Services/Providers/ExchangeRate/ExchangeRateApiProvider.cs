using NetCoreBackgroundJobsSample.Services.Dto;
using NetCoreBackgroundJobsSample.Services.Enum;
using NetCoreBackgroundJobsSample.Services.Providers.ExchangeRate.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.Services.Providers.ExchangeRate
{

    public class ExchangeRateApiProvider : BaseClient, ICurrencyProvider
    {
        private const string _exchangerateUrl = "https://api.exchangerate.host";

        public ExchangeRateApiProvider(IHttpClientFactory clientFactory) : base(clientFactory, new Uri(_exchangerateUrl))
        {
        }

        public async Task<GetCurrencyData> GetCurrencyDataAsync(CurrencyCode currencyCodePairOne, CurrencyCode currencyCodePairTwo)
        {
            var apiAction = string.Format("latest?base={0}&symbols={1}", currencyCodePairOne.ToString(), currencyCodePairTwo.ToString());
            var response = await GetAsync<ExchangeRateOutput>(apiAction);


            return response == null ? null : new GetCurrencyData()
            {
                Rate = response.rates.GetType().GetProperty(currencyCodePairTwo.ToString()).GetValue(response.rates).ToString(),
                Date = string.IsNullOrEmpty(response.date) ? DateTime.Now : Convert.ToDateTime(response.date)
            };
        }
    }
}
