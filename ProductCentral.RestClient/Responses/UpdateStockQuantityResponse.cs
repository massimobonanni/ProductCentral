namespace ProductCentral.RestClient.Responses
{
    public class UpdateStockQuantityResponse
    {
        public Guid ProductId { get; set; }
        public int NewStockQuantity { get; set; }
    }
}
