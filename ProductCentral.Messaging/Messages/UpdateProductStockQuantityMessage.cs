using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductCentral.Messaging.Messages
{
    public record UpdateProductStockQuantityMessage
    {
        [JsonPropertyName("productId")] public Guid ProductId { get; set; }
        [JsonPropertyName("stockQuantity")] public int StockQuantity { get; set; }
    }
}
