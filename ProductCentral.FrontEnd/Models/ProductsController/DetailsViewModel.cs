using ProductCentral.RestClient.Dto;

namespace ProductCentral.FrontEnd.Models.ProductsController;

/// <summary>
/// View model for displaying detailed product information in Razor Pages.
/// Contains the product data required for rendering product details views.
/// </summary>
public class DetailsViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the product data transfer object containing the detailed product information.
    /// </summary>
    public ProductDto Product { get; set; }
}
