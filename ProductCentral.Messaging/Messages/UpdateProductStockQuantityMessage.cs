using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCentral.Messaging.Messages
{
    public record UpdateProductStockQuantityMessage
    {
        public Guid ProductId { get; set; }
        public int StockQuantity { get; set; }
    }
}
