using ProductCentral.RestClient.Dto;

namespace ProductCentral.FrontEnd.Models.ProductsController;

/// <summary>
/// View model for the Products Index page, containing product data and search functionality.
/// </summary>
public class IndexViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the collection of products to display on the index page.
    /// </summary>
    public IEnumerable<ProductDto> Products { get; set; }

    /// <summary>
    /// Gets or sets the search term entered by the user to filter products.
    /// </summary>
    public string SearchTerm { get; set; }
}
