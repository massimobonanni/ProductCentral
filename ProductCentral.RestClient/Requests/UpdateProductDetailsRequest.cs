namespace ProductCentral.RestClient.Requests
{
    public class UpdateProductDetailsRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
