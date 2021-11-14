using System.Net.Http;
using Models;
using Newtonsoft.Json.Linq;
using Common;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class ExchangeDataService : IExchangeDataService
    {

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly ILogger<ExchangeDataService> logger;
        public ExchangeDataService(IHttpClientFactory httpClientFactory,
                                   IConfiguration config,
                                   ILogger<ExchangeDataService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = config;
            this.logger = logger;
        }
        
        //# FutureRefactoring
        // 1. Better and more informative exceptional handling should be done
        // 2. Async Await can be used, if found beneficial in load testing
        // 3. If business rules permits, exchange price data can be cached for short period of time
        public Result<ExchangePrice> GetExchangePrice(ConversionQuery query)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();
                var url = FormUri(query.SourceCurrency);
                var response = httpClient.GetAsync(url);
                var jsonString = response.Result.Content.ReadAsStringAsync();
                // dynamic used here so that, this parsing can be tolerant 
                //if there is slight change in response structure 
                // which does not changes interested properties
                dynamic priceObject = JObject.Parse(jsonString.Result);
                ExchangePrice exchangePrice = new ExchangePrice(priceObject);
                return new Result<ExchangePrice>(exchangePrice) { Success = true };
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex.Message);
                return new Result<ExchangePrice>(null)
                {
                    Success = false,
                    Error = "HttpRequestException occurred while getting exchange prices."
                };
            }
        }

        private Uri FormUri(string sourceCurrency)
        {
            var baseUri = new Uri(config.GetSection("BaseExchangeUrl").Value);
            string relativePath = config.GetSection("RelativeExchangeUrl").Value;
            string absoluteRelativePath = string.Format(relativePath, sourceCurrency.ToUpper());
            var compleletUri = new Uri(baseUri, absoluteRelativePath);
            return compleletUri;
        }
    }
}
