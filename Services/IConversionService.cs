using Models;
using Common;

namespace Services
{
    public interface IConversionService
    {
        Result<ConversionResult> ConvertCurrency(ConversionQuery query);       
    }
}
