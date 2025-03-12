using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Entities;
using ProductCentral.Core.Interfaces;
using ProductCentral.Core.Responses;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;

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
        var addProductDto =  JsonSerializer.Deserialize<AddProductRequest>(requestBody,
            new JsonSerializerOptions() {PropertyNameCaseInsensitive=true });

        if (addProductDto == null)
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync("Invalid product data.");
            return badRequestResponse;
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
            var createdResponse = req.CreateResponse(System.Net.HttpStatusCode.Created);
            await createdResponse.WriteAsJsonAsync(addProductResponse);
            return createdResponse;
        }
        else
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync(response.ErrorMessage);
            return badRequestResponse;
        }
    }
}