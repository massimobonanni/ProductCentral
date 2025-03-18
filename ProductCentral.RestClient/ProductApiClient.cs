using Microsoft.Extensions.Logging;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Net.Http.Json;

namespace ProductCentral.RestClient
{
    public class ProductApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductApiClient> _logger;
        private readonly string _apiKey;
        public readonly Uri _baseUri;

        public ProductApiClient(HttpClient httpClient, Uri baseUri, string apiKey, ILogger<ProductApiClient> logger)
        {
            _httpClient = httpClient;
            _baseUri = baseUri;
            _apiKey = apiKey;
            _logger = logger;
        }

        private string GetApiUrl(string apiSegment)
        {
            var uri = $"{_baseUri}{apiSegment}";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                return $"{uri}?code={_apiKey}";
            }
            return uri;
        }

        public async Task<AddProductResponse> AddProductAsync(AddProductRequest request)
        {
            var apiUrl = GetApiUrl("api/products");
            var response = await _httpClient.PostAsJsonAsync(apiUrl, request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AddProductResponse>();
        }

        public async Task<GetProductByIdResponse> GetProductByIdAsync(Guid productId)
        {
            var response = await _httpClient.GetAsync($"products/{productId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetProductByIdResponse>();
        }
    }
}
