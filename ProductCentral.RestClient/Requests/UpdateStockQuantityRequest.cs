namespace ProductCentral.RestClient.Requests;

/// <summary>
/// Represents a request to update the stock quantity of a product.
/// </summary>
public class UpdateStockQuantityRequest
{
    /// <summary>
    /// Gets or sets the quantity to add to the current stock.
    /// Use negative values to reduce stock quantity.
    /// </summary>
    public int StockQuantityToAdd { get; set; }
}
