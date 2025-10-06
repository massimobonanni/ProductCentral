using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Dto;
using ProductCentral.RestClient.Responses;

namespace ProductCentral.BackEnd.Functions;

/// <summary>
/// Azure Function for retrieving a specific product by its unique identifier.
/// Provides HTTP GET endpoint for product retrieval operations.
/// </summary>
public class GetProductByIdFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductByIdFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductByIdFunction"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository for data access operations.</param>
    /// <param name="logger">The logger for recording function execution information.</param>
    public GetProductByIdFunction(IProductRepository productRepository, ILogger<GetProductByIdFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a product by its unique identifier via HTTP GET request.
    /// </summary>
    /// <param name="req">The HTTP request data containing the request information.</param>
    /// <param name="productId">The unique identifier of the product to retrieve from the route parameter.</param>
    /// <returns>
    /// An HTTP response containing the product information if found, 
    /// or a 404 Not Found response if the product doesn't exist.
    /// </returns>
    [Function(nameof(GetProductById))]
    public async Task<HttpResponseData> GetProductById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{productId}")] HttpRequestData req,
        Guid productId)
    {
        _logger.LogInformation("Processing a request to get the product with ID {0}.", productId);

        var productResponse = await _productRepository.GetProductByIdAsync(productId, CancellationToken.None);
        if (!productResponse.Success)
        {
            return await req.CreateNotFoundResponseAsync(productResponse.ErrorMessage);
        }

        var productDto = new ProductDto(productResponse.Result);
        var response = new GetProductByIdResponse { Product = productDto };

        return await req.CreateOkResponseAsync(response);
    }
}
