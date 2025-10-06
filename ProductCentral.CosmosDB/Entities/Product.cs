using Newtonsoft.Json;

namespace ProductCentral.CosmosDB.Entities;


/// <summary>
/// Represents a product entity for Cosmos DB storage with mapping capabilities to core entities.
/// </summary>
internal class Product
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stock quantity of the product.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class from a core entity.
    /// </summary>
    /// <param name="source">The core product entity to copy data from.</param>
    public Product(Core.Entities.Product source)
    {
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        StockQuantity = source.StockQuantity;
        UnitPrice = source.UnitPrice;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class with a new unique identifier.
    /// </summary>
    public Product()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Converts this Cosmos DB product entity to a core product entity.
    /// </summary>
    /// <returns>A new core product entity with the same data as this instance.</returns>
    public Core.Entities.Product ToCoreEntity()
    {
        return new Core.Entities.Product
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            StockQuantity = this.StockQuantity,
            UnitPrice = this.UnitPrice
        };
    }
}
