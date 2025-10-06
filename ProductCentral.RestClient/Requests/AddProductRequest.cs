namespace ProductCentral.RestClient.Requests;

/// <summary>
/// Request object for adding a new product to the system.
/// Contains all the necessary information to create a new product.
/// </summary>
public class AddProductRequest
{
    /// <summary>
    /// Gets or sets the title of the product to be added.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the product to be added.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets the initial stock quantity for the product.
    /// </summary>
    public int StockQuantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
