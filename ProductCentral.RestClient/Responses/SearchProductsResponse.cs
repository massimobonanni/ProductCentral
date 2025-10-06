using ProductCentral.RestClient.Dto;

namespace ProductCentral.RestClient.Responses;

/// <summary>
/// Represents the response from a product search operation containing search results and metadata.
/// </summary>
public class SearchProductsResponse
{
    /// <summary>
    /// Gets or sets the search term that was used to find the products.
    /// </summary>
    public string SearchTerm { get; set; }

    /// <summary>
    /// Gets or sets the collection of products that match the search criteria.
    /// </summary>
    public IEnumerable<ProductDto> Products { get; set; }
}
