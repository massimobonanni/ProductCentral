using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductCentral.Messaging.Messages;

/// <summary>
/// Represents a message to update the stock quantity of a product.
/// </summary>
public record UpdateProductStockQuantityMessage
{
    /// <summary>
    /// Gets or sets the unique identifier of the product whose stock quantity is being updated.
    /// </summary>
    [JsonPropertyName("productId")] 
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the new stock quantity for the product.
    /// </summary>
    [JsonPropertyName("stockQuantity")] 
    public int StockQuantity { get; set; }
}
