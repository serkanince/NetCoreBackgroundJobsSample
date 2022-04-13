using NetCoreBackgroundJobsSample.Services.Dto;
using NetCoreBackgroundJobsSample.Services.Enum;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.Services
{
    public class CurrencyProviderContext
    {
        private readonly ICurrencyProvider _currencyProvider;

        public CurrencyProviderContext(ICurrencyProvider _currencyProvider)
        {
            this._currencyProvider = _currencyProvider;
        }

        public async Task<GetCurrencyData> GetCurrenctDataAsync(CurrencyCode currencyCodePairOne, CurrencyCode currencyCodePairTwo)
        {
            return await this._currencyProvider.GetCurrencyDataAsync(currencyCodePairOne, currencyCodePairTwo);
        }

    }
}
