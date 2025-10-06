using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Text.Json;

namespace ProductCentral.BackEnd.Functions;

/// <summary>
/// Azure Function for updating the stock quantity of a product.
/// </summary>
public class UpdateStockQuantityFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateStockQuantityFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStockQuantityFunction"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository for data operations.</param>
    /// <param name="logger">The logger for recording function execution information.</param>
    public UpdateStockQuantityFunction(IProductRepository productRepository, ILogger<UpdateStockQuantityFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Updates the stock quantity of a product by adding the specified amount to the current stock.
    /// </summary>
    /// <param name="req">The HTTP request containing the stock quantity update data.</param>
    /// <param name="productId">The unique identifier of the product to update.</param>
    /// <returns>
    /// An HTTP response containing the updated product information if successful,
    /// or an error response if the operation fails.
    /// </returns>
    [Function(nameof(UpdateStockQuantity))]
    public async Task<HttpResponseData> UpdateStockQuantity(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{productId}/update-stock")] HttpRequestData req,
        Guid productId)
    {
        _logger.LogInformation("Processing a request to update stock quantity for product {0}.", productId);

        var updateStockQuantityRequest = await req.GetRequestBodyAsync<UpdateStockQuantityRequest>();

        if (updateStockQuantityRequest == null)
        {
            return await req.CreateBadRequestResponseAsync("Invalid request data.");
        }

        var productResponse = await _productRepository.GetProductByIdAsync(productId, CancellationToken.None);
        if (!productResponse.Success)
        {
            return await req.CreateNotFoundResponseAsync("Product not found.");
        }

        var product = productResponse.Result;
        product.StockQuantity += updateStockQuantityRequest.StockQuantityToAdd;

        var updateResponse = await _productRepository.UpdateStockQuantityAsync(productId, product.StockQuantity, CancellationToken.None);
        if (!updateResponse.Success)
        {
            return await req.CreateBadRequestResponseAsync(updateResponse.ErrorMessage);
        }

        var updateStockQuantityResponse = new UpdateStockQuantityResponse
        {
            ProductId = productId,
            NewStockQuantity = product.StockQuantity
        };

        return await req.CreateOkResponseAsync(updateStockQuantityResponse);
    }
}
