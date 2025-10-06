namespace ProductCentral.RestClient.Responses;

/// <summary>
/// Represents the response returned after updating a product's stock quantity.
/// </summary>
public class UpdateStockQuantityResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the product whose stock quantity was updated.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the new stock quantity after the update operation.
    /// </summary>
    public int NewStockQuantity { get; set; }
}
