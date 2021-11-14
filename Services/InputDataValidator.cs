using Models;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Services
{
    public class InputDataValidator : IInputDataValidator
    {
        private readonly ILogger<InputDataValidator> logger;
        private readonly IConfiguration config;
        public InputDataValidator(ILogger<InputDataValidator> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        public ValidationResult ValidateInputData(ConversionQuery query)
        {
            // #FutureRefactoringOrImprovements : Separate rules classed can be implemented if complex rules are required. 
            // And then rule results can be collated to get combined result
            var validCurrencies = config.GetSection("ValidCurrencies:Currencies").Get<List<string>>();            
            if (query is null)
            {
                return ValidationResult.InValidInput;
            }

            if (string.IsNullOrWhiteSpace(query.SourceCurrency) || !validCurrencies.Contains(query.SourceCurrency.ToUpper()))
            {
                return ValidationResult.InvalidSourceCurrency;
            }

            if (string.IsNullOrWhiteSpace(query.TargetCurrency) || !validCurrencies.Contains(query.TargetCurrency.ToUpper()))
            {
                return ValidationResult.InvalidTargetCurrency;
            }

            try
            {
                var priceValue = Convert.ToDecimal(query.Price);
            }
            catch (OverflowException)
            {
                return ValidationResult.PriceOverFlow;
            }
            catch (Exception)
            {
                return ValidationResult.InvalidPrice;
            }
            logger.LogInformation("Input data is valid.");
            return ValidationResult.Valid;
        }
    }
}
