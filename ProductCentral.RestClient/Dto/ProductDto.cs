using ProductCentral.Core.Entities;

namespace ProductCentral.RestClient.Dto;


public record ProductDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }


    public int StockQuantity { get; set; }

    public decimal UnitPrice { get; set; }

    public ProductDto()
    {

    }

    public ProductDto(Product product)
    {
        this.Id = product.Id;
        this.Title = product.Title;
        this.Description = product.Description;
        this.StockQuantity = product.StockQuantity;
        this.UnitPrice = product.UnitPrice;
    }
}
