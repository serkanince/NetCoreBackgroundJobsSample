using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.Services.Providers.ExchangeRate.Dto
{
    public class ExchangeRateOutput
    {
        public Motd motd { get; set; }
        public bool success { get; set; }
        public string _base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }
    }

    public class Motd
    {
        public string msg { get; set; }
        public string url { get; set; }
    }

    public class Rates
    {
        public float TRY { get; set; }
    }


}
