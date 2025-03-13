using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.Messaging.Messages;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Text.Json;

namespace ProductCentral.BackEnd.Functions;

public class UpdateStockQuantityFromServiceBusFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateStockQuantityFromServiceBusFunction> _logger;

    public UpdateStockQuantityFromServiceBusFunction(IProductRepository productRepository, ILogger<UpdateStockQuantityFromServiceBusFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    [Function(nameof(UpdateStockQuantityFromServiceBus))]
    public async Task UpdateStockQuantityFromServiceBus(
        [ServiceBusTrigger("%ServiceBusTopicName%", "%ServiceBusSubscriptionName%", Connection = "ServiceBusConnectionString")] string message)
    {
        _logger.LogInformation("Processing a message to update stock quantity.");

        var updateStockQuantityRequest = JsonSerializer.Deserialize<UpdateProductStockQuantityMessage>(message);

        if (updateStockQuantityRequest == null)
        {
            _logger.LogError("Invalid message data.");
            return;
        }

        var productResponse = await _productRepository.GetProductByIdAsync(updateStockQuantityRequest.ProductId, CancellationToken.None);
        if (!productResponse.Success)
        {
            _logger.LogError("Product not found.");
            return;
        }

        var product = productResponse.Result;
        product.StockQuantity += updateStockQuantityRequest.StockQuantity;

        var updateResponse = await _productRepository.UpdateStockQuantityAsync(updateStockQuantityRequest.ProductId, product.StockQuantity, CancellationToken.None);
        if (!updateResponse.Success)
        {
            _logger.LogError(updateResponse.ErrorMessage);
            return;
        }

        _logger.LogInformation("Stock quantity updated successfully for product {0}.", updateStockQuantityRequest.ProductId);
    }
}
