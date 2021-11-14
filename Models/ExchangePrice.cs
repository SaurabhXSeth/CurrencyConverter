namespace Models
{
    public class ExchangePrice
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public string TimeLastUpdated { get; set; }
        public System.Collections.Generic.Dictionary<string, decimal> Rates { get; set; }

        public ExchangePrice()
        {
        }
        public ExchangePrice(dynamic exchangeData)
        {
            Base = exchangeData["base"];
            Date = exchangeData["date"];
            TimeLastUpdated = exchangeData["time_last_updated"];
            Rates = exchangeData["rates"].ToObject<System.Collections.Generic.Dictionary<string, decimal>>();
        }
    }
}

