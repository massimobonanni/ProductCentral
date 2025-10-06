using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using System.Net;

namespace ProductCentral.BackEnd.Functions;

/// <summary>
/// Azure Function for deleting products from the product repository.
/// </summary>
public class DeleteProductFunction
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductFunction"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository for data operations.</param>
    /// <param name="logger">The logger for recording function execution information.</param>
    public DeleteProductFunction(IProductRepository productRepository, ILogger<DeleteProductFunction> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="req">The HTTP request data.</param>
    /// <param name="productId">The unique identifier of the product to delete.</param>
    /// <returns>
    /// An HTTP response indicating the result of the delete operation:
    /// - 204 No Content if the product was successfully deleted
    /// - 404 Not Found if the product does not exist
    /// - 400 Bad Request if the delete operation failed
    /// </returns>
    [Function(nameof(DeleteProduct))]
    public async Task<HttpResponseData> DeleteProduct(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{productId}")] HttpRequestData req,
        Guid productId)
    {
        _logger.LogInformation("Processing a request to delete product with ID {0}.", productId);

        var productResponse = await _productRepository.GetProductByIdAsync(productId, CancellationToken.None);
        if (!productResponse.Success || productResponse.Result == null)
        {
            return await req.CreateNotFoundResponseAsync("Product not found.");
        }

        var deleteResponse = await _productRepository.DeleteProductAsync(productId, CancellationToken.None);

        if (!deleteResponse.Success)
        {
            return await req.CreateStringResponseAsync(HttpStatusCode.BadRequest, deleteResponse.ErrorMessage);
        }

        return await req.CreateResponseAsync(HttpStatusCode.NoContent);
    }
}
