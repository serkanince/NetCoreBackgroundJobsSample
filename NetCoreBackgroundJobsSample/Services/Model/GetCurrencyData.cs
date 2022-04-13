using System;

namespace NetCoreBackgroundJobsSample.Services.Dto
{
    public class GetCurrencyData
    {
        public string Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
