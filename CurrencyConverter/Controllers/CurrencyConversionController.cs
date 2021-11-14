using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using Microsoft.AspNetCore.Http;
using Common;
using System;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // #FutureRefactoringOrImprovements 
    // 1. Authentication and authorization should be added so that only authorized users can call the API
    // 2. Health check endpoint should be added
    // 3. Rounding and trimming of returned value
    // 4. A better third party logger can be implemented, currently logs are in debug window
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ILogger<CurrencyConversionController> logger;
        private readonly IConversionService conversionService;
        private readonly IInputDataValidator inputDataValidator;
        public CurrencyConversionController(ILogger<CurrencyConversionController> logger,
            IConversionService conversionService,
            IInputDataValidator inputDataValidator)
        {
            this.logger = logger;
            this.conversionService = conversionService;
            this.inputDataValidator = inputDataValidator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConversionResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ConvertPrice(ConversionQuery query)
        {            
            try
            {
                var validationResult = inputDataValidator.ValidateInputData(query);
                if (validationResult == ValidationResult.InValidInput
                    || validationResult == ValidationResult.InvalidPrice
                    || validationResult == ValidationResult.InvalidSourceCurrency
                    || validationResult == ValidationResult.InvalidTargetCurrency
                    || validationResult == ValidationResult.PriceOverFlow)
                {
                    return BadRequest(validationResult.Description());
                }
                var conversionResult = conversionService.ConvertCurrency(query);
                if (conversionResult.Success)
                {
                    return Ok(conversionResult.Data);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
            return StatusCode(500);
        }
    }
}
