using ProductCentral.RestClient.Dto;

namespace ProductCentral.RestClient.Responses;

public class SearchProductsResponse
{
    public string SearchTerm { get; set; }

    public IEnumerable<ProductDto> Products { get; set; }
}
