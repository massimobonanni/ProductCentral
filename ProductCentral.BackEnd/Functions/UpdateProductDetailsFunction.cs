using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.RestClient.Requests;
using ProductCentral.RestClient.Responses;
using System.Text.Json;

namespace ProductCentral.BackEnd.Functions
{
    public class UpdateProductDetailsFunction
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateProductDetailsFunction> _logger;

        public UpdateProductDetailsFunction(IProductRepository productRepository, ILogger<UpdateProductDetailsFunction> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [Function(nameof(UpdateProductDetails))]
        public async Task<HttpResponseData> UpdateProductDetails(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products/{productId}")] HttpRequestData req,
            Guid productId)
        {
            _logger.LogInformation("Processing a request to update product details for product {0}.", productId);

            var updateProductDetailRequest = await req.GetRequestBodyAsync<UpdateProductDetailsRequest>();

            if (updateProductDetailRequest == null)
            {
                return await req.CreateBadRequestResponseAsync("Invalid request data.");
            }

            var updateResponse = await _productRepository.UpdateProductDetailsAsync(
                productId,
                updateProductDetailRequest.Title,
                updateProductDetailRequest.Description,
                updateProductDetailRequest.UnitPrice,
                CancellationToken.None);

            if (!updateResponse.Success)
            {
                return await req.CreateBadRequestResponseAsync(updateResponse.ErrorMessage);
            }

            var updateProductDetailResponse = new UpdateProductDetailsResponse
            {
                Id = productId
            };

            return await req.CreateOkResponseAsync(updateProductDetailResponse);
        }
    }
}
