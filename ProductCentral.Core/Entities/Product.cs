using System;

namespace ProductCentral.Core.Entities;

/// <summary>
/// Represents a product with details such as title, description, stock quantity, and unit price.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
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
    /// Gets or sets the stock quantity of the product.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class with a new unique identifier.
    /// </summary>
    public Product()
    {
        Id = Guid.NewGuid();
    }
}
