using ProductCentral.RestClient.Dto;

namespace ProductCentral.FrontEnd.Models.ProductsController
{
    public class IndexViewModel: ViewModelBase
    {
        public IEnumerable<ProductDto> Products { get; set; }

        public string SearchTerm { get; set; }
    }
}
