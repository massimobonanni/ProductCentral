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

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updateStockQuantityRequest = JsonSerializer.Deserialize<UpdateStockQuantityRequest>(requestBody,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        if (updateStockQuantityRequest == null)
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync("Invalid request data.");
            return badRequestResponse;
        }

        var productResponse = await _productRepository.GetProductByIdAsync(productId, CancellationToken.None);
        if (!productResponse.Success)
        {
            var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            await notFoundResponse.WriteStringAsync("Product not found.");
            return notFoundResponse;
        }

        var product = productResponse.Result;
        product.StockQuantity += updateStockQuantityRequest.StockQuantityToAdd;

        var updateResponse = await _productRepository.UpdateStockQuantityAsync(productId, product.StockQuantity, CancellationToken.None);
        if (!updateResponse.Success)
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync(updateResponse.ErrorMessage);
            return badRequestResponse;
        }

        var updateStockQuantityResponse = new UpdateStockQuantityResponse
        {
            ProductId = productId,
            NewStockQuantity = product.StockQuantity
        };

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(updateStockQuantityResponse);
        return response;
    }
}
