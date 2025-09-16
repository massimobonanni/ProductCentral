using ProductCentral.Core.Entities;
using System.Text.Json.Serialization;

namespace ProductCentral.BackEnd.Models;

public class ProductAddedEvent(Product product) : EventBase
{
    public Guid ProductId { get; set; } = product.Id;
    public string Title { get; set; } = product.Title;
    public string Description { get; set; } = product.Description;
    public int StockQuantity { get; set; } = product.StockQuantity;
    public decimal UnitPrice { get; set; } = product.UnitPrice;

    [JsonIgnore()] 
    public override string EventType => "ProductCentral.Product.Added";
    [JsonIgnore()] 
    public override string DataVersion => "1.0";
    [JsonIgnore()] 
    public override string Subject => $"products/{product.Id}";
}