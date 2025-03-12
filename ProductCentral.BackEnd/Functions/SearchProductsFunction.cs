using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Dto;
using ProductCentral.RestClient.Responses;

namespace ProductCentral.BackEnd.Functions;

public class SearchProductsFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<SearchProductsFunction> _logger;

    public SearchProductsFunction(IProductRepository productRepository, ILogger<SearchProductsFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

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
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync(searchResponse.ErrorMessage);
            return badRequestResponse;
        }

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new SearchProductsResponse
            {
                SearchTerm = searchTerm,
                Products = searchResponse.Result.Select(p => new ProductDto(p))
            }
        );
        return response;
    }
}
