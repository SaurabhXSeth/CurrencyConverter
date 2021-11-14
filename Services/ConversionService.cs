using Models;
using System;
using Data;
using Microsoft.Extensions.Logging;
using Common;

namespace Services
{
    public class ConversionService : IConversionService
    {
        private readonly ILogger<ConversionService> logger;
        private readonly IExchangeDataService exchnageDataService;

        public ConversionService(ILogger<ConversionService> logger,
            IExchangeDataService exchnageDataService)
        {
            this.logger = logger;
            this.exchnageDataService = exchnageDataService;

        }

        public Result<ConversionResult> ConvertCurrency(ConversionQuery query)
        {
            var retValue = exchnageDataService.GetExchangePrice(query);
            if (retValue.Success)
            {                
                decimal conversionRate = retValue.Data.Rates[query.TargetCurrency];
                decimal sourcePrice = Convert.ToDecimal(query.Price);
                ConversionResult result = new ConversionResult
                {
                    //#FutureRefactoringOrImprovements : Try to pass exact error of overflow to caller, if overflow happens here.
                    ConvertedPrice = sourcePrice * conversionRate,
                    TargetCurrency = query.TargetCurrency
                };

                logger.LogInformation("Price converted successfully.");
                return new Result<ConversionResult>(result) { Success = true};
            }
            else
            {
                return new Result<ConversionResult>(null)
                {
                    Success = false,
                    Error = retValue.Error
                };  
            }
        }
    }
}
