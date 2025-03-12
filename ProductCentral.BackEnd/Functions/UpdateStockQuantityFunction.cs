using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Text.Json;

namespace ProductCentral.BackEnd.Functions;

public class UpdateStockQuantityFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateStockQuantityFunction> _logger;

    public UpdateStockQuantityFunction(IProductRepository productRepository, ILogger<UpdateStockQuantityFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

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
