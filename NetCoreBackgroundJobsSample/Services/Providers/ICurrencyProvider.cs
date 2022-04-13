using NetCoreBackgroundJobsSample.Services.Dto;
using NetCoreBackgroundJobsSample.Services.Enum;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.Services
{
    public interface ICurrencyProvider
    {
        Task<GetCurrencyData> GetCurrencyDataAsync(CurrencyCode baseCurrency, CurrencyCode symbolCurrency);
    }
}
