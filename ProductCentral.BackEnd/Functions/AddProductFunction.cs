using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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

    public AddProductFunction(IProductRepository productRepository, ILogger<AddProductFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
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
            var addProductResponse = new AddProductResponse { Id = product.Id };
            return await req.CreateResponseAsync(System.Net.HttpStatusCode.Created, addProductResponse);
        }
        else
        {
            return await req.CreateBadRequestResponseAsync(response.ErrorMessage);
        }
    }
}