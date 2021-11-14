using Models;
using Common;

namespace Data
{
    public interface IExchangeDataService
    {
        Result<ExchangePrice> GetExchangePrice(ConversionQuery query);
    }
}
