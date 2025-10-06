using ProductCentral.Core.Entities;

namespace ProductCentral.RestClient.Dto;

/// <summary>
/// Data Transfer Object representing a product for API communication.
/// Used to transfer product information between client and server.
/// </summary>
public record ProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the current stock quantity of the product.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDto"/> record.
    /// </summary>
    public ProductDto()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDto"/> record from a Product entity.
    /// </summary>
    /// <param name="product">The Product entity to create the DTO from.</param>
    public ProductDto(Product product)
    {
        this.Id = product.Id;
        this.Title = product.Title;
        this.Description = product.Description;
        this.StockQuantity = product.StockQuantity;
        this.UnitPrice = product.UnitPrice;
    }
}
