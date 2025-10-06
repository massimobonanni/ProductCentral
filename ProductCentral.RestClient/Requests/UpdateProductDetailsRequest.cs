namespace ProductCentral.RestClient.Requests;

/// <summary>
/// Represents a request to update product details including title, description, and unit price.
/// </summary>
public class UpdateProductDetailsRequest
{
    /// <summary>
    /// Gets or sets the product title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
