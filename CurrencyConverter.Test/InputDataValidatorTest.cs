using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Configuration;
using Models;

namespace ClaimsAccumulator.Test
{
    public class InputDataValidatorTest
    {

        private readonly InputDataValidator sut;
        private readonly Mock<ILogger<InputDataValidator>> mockLogger;
        private readonly Mock<IConfiguration> mockConfig;

        public InputDataValidatorTest()
        {
            mockLogger = new Mock<ILogger<InputDataValidator>>();
            mockConfig = new Mock<IConfiguration>();
            
            var gbpSectionMock = new Mock<IConfigurationSection>();
            gbpSectionMock.Setup(s => s.Value).Returns("GBP");
            var eurSectionMock = new Mock<IConfigurationSection>();
            eurSectionMock.Setup(s => s.Value).Returns("EUR");
            var currencySectionMock = new Mock<IConfigurationSection>();
            currencySectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { gbpSectionMock.Object, eurSectionMock.Object });            mockConfig.Setup(c => c.GetSection("ValidCurrencies:Currencies")).Returns(currencySectionMock.Object);


            sut = new InputDataValidator(mockLogger.Object, mockConfig.Object);
        }

        [Fact]
        public void Should_Save_All_Valid_Claim_Collection()
        {
            // Prepare
            ConversionQuery query = new ConversionQuery() { Price = "100", SourceCurrency = "GBP", TargetCurrency = "EUR" };

            //Execute
            var result = sut.ValidateInputData(query);

            //Assert
            result.Should().Be(ValidationResult.Valid);
        }

        [Fact]
        public void Should_Return_Invalid_Currency()
        {
            // Prepare
            ConversionQuery query = new ConversionQuery() { Price = "100", SourceCurrency = "GBP1", TargetCurrency = "EUR" };

            //Execute
            var result = sut.ValidateInputData(query);

            //Assert
            result.Should().Be(ValidationResult.InvalidSourceCurrency);
        }




    }
}

