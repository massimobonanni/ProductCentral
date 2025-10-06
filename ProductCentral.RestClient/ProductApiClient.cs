using Microsoft.Extensions.Logging;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Net.Http.Json;

namespace ProductCentral.RestClient
{
    /// <summary>
    /// HTTP client for interacting with the ProductCentral API.
    /// Provides methods for product management operations including adding, retrieving, and searching products.
    /// </summary>
    public class ProductApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductApiClient> _logger;
        private readonly string _apiKey;
        public readonly Uri _baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for API requests.</param>
        /// <param name="baseUri">The base URI of the ProductCentral API.</param>
        /// <param name="apiKey">The API key for authentication with the API.</param>
        /// <param name="logger">Logger for recording client operation information.</param>
        public ProductApiClient(HttpClient httpClient, Uri baseUri, string apiKey, ILogger<ProductApiClient> logger)
        {
            _httpClient = httpClient;
            _baseUri = baseUri;
            _apiKey = apiKey;
            _logger = logger;
        }

        /// <summary>
        /// Constructs the full API URL for a given endpoint, including API key and query parameters.
        /// </summary>
        /// <param name="apiSegment">The API endpoint segment (e.g., "api/products").</param>
        /// <param name="queryString">Optional query string parameters.</param>
        /// <returns>The complete API URL with authentication and query parameters.</returns>
        private string GetApiUrl(string apiSegment, string queryString = null)
        {
            var uri = $"{_baseUri}{apiSegment}";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                uri = $"{uri}?code={_apiKey}";
            }
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                if (uri.Contains("?"))
                    uri = $"{uri}&{queryString}";
                else
                    uri = $"{uri}?{queryString}";
            }
            return uri;
        }

        /// <summary>
        /// Adds a new product to the system via the API.
        /// </summary>
        /// <param name="request">The request containing the product details to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the created product ID.</returns>
        /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
        public async Task<AddProductResponse> AddProductAsync(AddProductRequest request)
        {
            var apiUrl = GetApiUrl("api/products");
            var response = await _httpClient.PostAsJsonAsync(apiUrl, request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AddProductResponse>();
        }

        /// <summary>
        /// Retrieves a specific product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the product details.</returns>
        /// <exception cref="HttpRequestException">Thrown when the API request fails or the product is not found.</exception>
        public async Task<GetProductByIdResponse> GetProductByIdAsync(Guid productId)
        {
            var apiUrl = GetApiUrl($"api/products/{productId}");
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetProductByIdResponse>();
        }

        /// <summary>
        /// Searches for products based on the provided search criteria.
        /// </summary>
        /// <param name="searchText">Optional search text to filter products by title or description. If null or empty, returns all products.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching products.</returns>
        /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
        public async Task<SearchProductsResponse> SearchProductsAsync(string searchText = null)
        {
            string apiUrl = null;
            if (string.IsNullOrWhiteSpace(searchText))
                apiUrl = GetApiUrl($"api/products");
            else
                apiUrl = GetApiUrl($"api/products", $"searchTerm ={searchText}");
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SearchProductsResponse>();
        }
    }
}
