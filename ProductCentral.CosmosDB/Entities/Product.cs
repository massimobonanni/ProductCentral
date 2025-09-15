using Newtonsoft.Json;

namespace ProductCentral.CosmosDB.Entities;


internal class Product
{

    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }


    public string Title { get; set; } = string.Empty;


    public string Description { get; set; } = string.Empty;


    public int StockQuantity { get; set; }


    public decimal UnitPrice { get; set; }


    public Product(Core.Entities.Product source)
    {
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        StockQuantity = source.StockQuantity;
        UnitPrice = source.UnitPrice;
    }

    public Product()
    {
        Id = Guid.NewGuid();
    }

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
