using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Dto;
using ProductCentral.RestClient.Responses;

namespace ProductCentral.BackEnd.Functions
{
    public class GetProductByIdFunction
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductByIdFunction> _logger;

        public GetProductByIdFunction(IProductRepository productRepository, ILogger<GetProductByIdFunction> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

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
}
