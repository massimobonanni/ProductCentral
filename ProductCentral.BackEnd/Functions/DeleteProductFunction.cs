using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using System.Net;

namespace ProductCentral.BackEnd.Functions
{
    public class DeleteProductFunction
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteProductFunction> _logger;

        public DeleteProductFunction(IProductRepository productRepository, ILogger<DeleteProductFunction> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [Function(nameof(DeleteProduct))]
        public async Task<HttpResponseData> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{productId}")] HttpRequestData req,
            Guid productId)
        {
            _logger.LogInformation("Processing a request to delete product with ID {0}.", productId);

            var productResponse = await _productRepository.GetProductByIdAsync(productId, CancellationToken.None);
            if (!productResponse.Success || productResponse.Result == null)
            {
                return await req.CreateNotFoundResponseAsync( "Product not found.");
            }

            var deleteResponse = await _productRepository.DeleteProductAsync(productId, CancellationToken.None);

            if (!deleteResponse.Success)
            {
                return await req.CreateStringResponseAsync(HttpStatusCode.BadRequest, deleteResponse.ErrorMessage);
            }

            return await req.CreateResponseAsync(HttpStatusCode.NoContent);
        }
    }
}
