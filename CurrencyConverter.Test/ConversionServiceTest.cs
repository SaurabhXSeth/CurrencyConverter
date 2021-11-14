using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using System.Collections.Generic;
using Xunit;
using Data;

namespace CurrencyConverter.Test
{
    /// <summary>
    /// #FutureRefactoringOrImprovements
    /// 1. More tests can be written to cover all edge cases
    /// </summary>
    public class ConversionServiceTest
    {

        private readonly ConversionService sut;
        private readonly Mock<IExchangeDataService> mockExchangeDataService;
        private readonly Mock<ILogger<ConversionService>> mockLogger;

        public ConversionServiceTest()
        {
            mockExchangeDataService = new Mock<IExchangeDataService>();
            mockLogger = new Mock<ILogger<ConversionService>>();

            sut = new ConversionService(mockLogger.Object, mockExchangeDataService.Object);
        }

        [Fact]
        public void Should_Convert_GBP_to_EUR()
        {
            // Prepare
            mockExchangeDataService.Setup(x => x.GetExchangePrice(It.IsAny<Models.ConversionQuery>()))
                .Returns(new Common.Result<Models.ExchangePrice>(new Models.ExchangePrice()
                {
                    Base = "GBP",
                    Rates = new Dictionary<string, decimal>() { { "EUR", 1.2m } }
                }));

            //Execute
            var result = sut.ConvertCurrency(new Models.ConversionQuery()
            { Price = "100", SourceCurrency = "GBP", TargetCurrency = "EUR" });

            //Assert
            result.Data.ConvertedPrice.Should().Be(120.0m);
        }
    }
}

