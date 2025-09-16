using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.BackEnd.Models;
using ProductCentral.Core.Entities;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Text.Json;

namespace ProductCentral.BackEnd.Functions;

public class AddProductFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<AddProductFunction> _logger;
    private readonly EventGridPublisherClient _eventGridClient;

    public AddProductFunction(IProductRepository productRepository, ILogger<AddProductFunction> logger,
        EventGridPublisherClient eventGridClient)
    {
        _productRepository = productRepository;
        _logger = logger;
        _eventGridClient = eventGridClient;
    }

    [Function(nameof(AddProduct))]
    public async Task<HttpResponseData> AddProduct(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequestData req)
    {
        _logger.LogInformation("Processing a request to add a new product.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            return await req.CreateBadRequestResponseAsync("Request body cannot be empty.");
        }

        var addProductDto = JsonSerializer.Deserialize<AddProductRequest>(requestBody,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        if (addProductDto == null)
        {
            return await req.CreateBadRequestResponseAsync("Invalid product data.");
        }

        var product = new Product
        {
            Title = addProductDto.Title,
            Description = addProductDto.Description,
            StockQuantity = addProductDto.StockQuantity,
            UnitPrice = addProductDto.UnitPrice
        };

        var response = await _productRepository.AddProductAsync(product, CancellationToken.None);

        if (response.Success)
        {
            // Publish event to Event Grid
            try
            {
                var productAddedEvent = new ProductAddedEvent(product);
                await _eventGridClient.SendEventAsync(productAddedEvent.ToEventGridEvent());
                _logger.LogInformation("Successfully published ProductAdded event for product {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish ProductAdded event for product {ProductId}", product.Id);
            }

            var addProductResponse = new AddProductResponse { Id = product.Id };
            return await req.CreateResponseAsync(System.Net.HttpStatusCode.Created, addProductResponse);
        }
        else
        {
            return await req.CreateBadRequestResponseAsync(response.ErrorMessage);
        }
    }
}