using NetCoreBackgroundJobsSample.Services.Dto;
using NetCoreBackgroundJobsSample.Services.Enum;
using NetCoreBackgroundJobsSample.Services.Model;
using NetCoreBackgroundJobsSample.Services.Providers;
using NetCoreBackgroundJobsSample.Services.Providers.TCMB.Model;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NetCoreBackgroundJobsSample.Services
{
    public class TCMBApiProvider : BaseClient, ICurrencyProvider
    {
        private const string _tcmbUrl = "http://www.tcmb.gov.tr/";

        public TCMBApiProvider(IHttpClientFactory clientFactory) : base(clientFactory, new Uri(_tcmbUrl))
        {
        }

        private TCMBTodayOutput GetCurrencyDataAsync(string response, CurrencyCode currencyCode)
        {
            XDocument xDoc = XDocument.Parse(response);
            XElement xe = xDoc.Element("Tarih_Date");

            if (xe != null)
            {
                DateTime date = DateTime.ParseExact(xe.Attribute("Date").Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                int i = 0;
                foreach (XElement xeCurrency in xe.Elements())
                {
                    StringReader reader = new StringReader(xeCurrency.ToString());
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Currency));
                    var currency = (Currency)xmlSerializer.Deserialize(reader);

                    if (currency.CurrencyCode == currencyCode.ToString())
                    {
                        return new TCMBTodayOutput() { Currency = currency };
                    }

                    return null;
                }
            }

            return new TCMBTodayOutput();
        }

        public async Task<GetCurrencyData> GetCurrencyDataAsync(CurrencyCode currencyCodePairOne, CurrencyCode currencyCodePairTwo)
        {
            var response = await GetAsync("kurlar/today.xml");
            TCMBTodayOutput currency = GetCurrencyDataAsync(response, currencyCodePairOne);

            return currency == null ? null : new GetCurrencyData()
            {
                Rate = currency.Currency.ForexSelling.ToString(),
                Date = DateTime.Now
            };
        }
    }
}
