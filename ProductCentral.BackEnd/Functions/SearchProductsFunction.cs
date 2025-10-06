using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Dto;
using ProductCentral.RestClient.Responses;

namespace ProductCentral.BackEnd.Functions;

/// <summary>
/// Azure Function for searching products based on a search term.
/// Provides HTTP endpoint to query products from the repository.
/// </summary>
public class SearchProductsFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<SearchProductsFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchProductsFunction"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository for accessing product data.</param>
    /// <param name="logger">The logger for logging function execution information.</param>
    public SearchProductsFunction(IProductRepository productRepository, ILogger<SearchProductsFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Searches for products based on a search term provided in the query string.
    /// </summary>
    /// <param name="req">The HTTP request data containing the search term in the query string.</param>
    /// <returns>A task representing the asynchronous operation, with HTTP response data containing the search results or an error response.</returns>
    [Function(nameof(SearchProducts))]
    public async Task<HttpResponseData> SearchProducts(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequestData req)
    {
        _logger.LogInformation("Processing a request to search for products.");

        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string searchTerm = query["searchTerm"];

        var searchResponse = await _productRepository.SearchProductsAsync(searchTerm, CancellationToken.None);
        if (!searchResponse.Success)
        {
            return await req.CreateBadRequestResponseAsync(searchResponse.ErrorMessage);
        }

        return await req.CreateOkResponseAsync(new SearchProductsResponse
        {
            SearchTerm = searchTerm,
            Products = searchResponse.Result.Select(p => new ProductDto(p))
        }
        );
    }
}
