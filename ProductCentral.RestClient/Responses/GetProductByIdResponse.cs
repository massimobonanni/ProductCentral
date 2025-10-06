using ProductCentral.RestClient.Dto;

namespace ProductCentral.RestClient.Responses;

/// <summary>
/// Represents the response from getting a product by its identifier.
/// Contains the requested product data transfer object.
/// </summary>
public class GetProductByIdResponse
{
    /// <summary>
    /// Gets or sets the product data returned from the API request.
    /// </summary>
    public ProductDto Product { get; set; }
}
