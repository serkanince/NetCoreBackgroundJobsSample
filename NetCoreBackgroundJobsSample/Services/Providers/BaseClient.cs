using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreBackgroundJobsSample.Services.Providers
{
    public class BaseClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public BaseClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("currencyClient");
        }
        public BaseClient(IHttpClientFactory clientFactory, Uri uri) : this(clientFactory)
        {
            _httpClient.BaseAddress = uri;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            var response = await _httpClient.GetStringAsync(path);

            return JsonSerializer.Deserialize<T>(response);
        }
        public async Task<string> GetAsync(string path)
        {
            return await _httpClient.GetStringAsync(path);
        }
    }
}
