using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FundaAssignment.Core.Providers
{
    public class FundaProvider : IProvider
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(FundaProvider));
        private const string _baseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type={0}&zo=/{1}{2}&page={3}&pagesize={4}";
        private const string _query = "/{0}/";
        private const int _batchSize = 25;
        private readonly HttpClient _httpClient = new HttpClient();

        public IEnumerable<Property> Get(RequestModel request)
        {
            List<Property> properties = new List<Property>();

            var firstCallUrl = BuildRequestUrl(request, 1);
            var firstResponse = GetDataFromFunda(firstCallUrl).Result;

            if (firstResponse == null)
                return properties;

            properties.AddRange(firstResponse.Objects);
            
            if (!properties.Any())
                return properties;

            var totalPages = firstResponse.Paging.AantalPaginas;
            
            if (totalPages < 2)
                return properties;

            var tasks = new List<Task<ResponseModel>>();
            tasks.AddRange(Enumerable.Range(2, totalPages)
                .Select(page => GetDataFromFunda(BuildRequestUrl(request, page))));

            var responseModels = Task.WhenAll(tasks.ToArray()).Result;

            foreach (var responseModel in responseModels)
            {
                if (responseModel != null)
                    properties.AddRange(responseModel.Objects);
            }

            return properties;
        }

        private async Task<ResponseModel> GetDataFromFunda(string url)
        {
            ResponseModel response;
            try
            {
                var responseString = await _httpClient.GetStringAsync(url);
                response = JsonConvert.DeserializeObject<ResponseModel>(responseString);

                return response;
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Too many requests, requires retry after timeout. Failed with following message: {ex.Message}");
                Thread.Sleep(1000);
                return await GetDataFromFunda(url);
            }
            catch (Exception ex)
            {
                _log.Error($"Request has failed with following message: {ex.Message}");
                return null;
            }
        }

        private string BuildRequestUrl(RequestModel request, int page) 
        {
            var query = string.IsNullOrEmpty(request.Buitenruimte) ?
                string.Empty : string.Format(_query, request.Buitenruimte);

            var url = string.Format(_baseUrl, request.Type, request.Stad, query, page, _batchSize);

            return url;
        }
    }
}
