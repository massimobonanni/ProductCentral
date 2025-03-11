namespace ProductCentral.RestClient.Requests;

public class AddProductRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
}
