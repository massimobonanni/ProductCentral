namespace ProductCentral.RestClient.Responses;

/// <summary>
/// Response model for adding a new product operation.
/// </summary>
public class AddProductResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly created product.
    /// </summary>
    public Guid Id { get; set; }
}
