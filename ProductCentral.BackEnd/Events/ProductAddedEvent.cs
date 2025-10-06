using ProductCentral.Core.Entities;
using System.Text.Json.Serialization;

namespace ProductCentral.BackEnd.Models;

/// <summary>
/// Event raised when a new product is added to the system.
/// Contains all the relevant product information for event subscribers.
/// </summary>
/// <param name="product">The product that was added to the system.</param>
public class ProductAddedEvent(Product product) : EventBase
{
    /// <summary>
    /// Gets or sets the unique identifier of the added product.
    /// </summary>
    public Guid ProductId { get; set; } = product.Id;
    
    /// <summary>
    /// Gets or sets the title of the added product.
    /// </summary>
    public string Title { get; set; } = product.Title;
    
    /// <summary>
    /// Gets or sets the description of the added product.
    /// </summary>
    public string Description { get; set; } = product.Description;
    
    /// <summary>
    /// Gets or sets the initial stock quantity of the added product.
    /// </summary>
    public int StockQuantity { get; set; } = product.StockQuantity;
    
    /// <summary>
    /// Gets or sets the unit price of the added product.
    /// </summary>
    public decimal UnitPrice { get; set; } = product.UnitPrice;

    /// <summary>
    /// Gets the event type identifier for product addition events.
    /// </summary>
    [JsonIgnore()] 
    public override string EventType => "ProductCentral.Product.Added";
    
    /// <summary>
    /// Gets the data version for this event type.
    /// </summary>
    [JsonIgnore()] 
    public override string DataVersion => "1.0";
    
    /// <summary>
    /// Gets the subject identifier for this specific product event.
    /// </summary>
    [JsonIgnore()] 
    public override string Subject => $"products/{product.Id}";
}