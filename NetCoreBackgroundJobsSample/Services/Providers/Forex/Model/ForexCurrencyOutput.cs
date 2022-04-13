namespace NetCoreBackgroundJobsSample.Services.Forex.Dto
{
    public class ForexCurrencyOutput
    {
        public Rates rates { get; set; }
        public int code { get; set; }
    }

    public class Rates
    {
        public USDTRY USDTRY { get; set; }
    }

    public class USDTRY
    {
        public float rate { get; set; }
        public double timestamp { get; set; }
    }

}
