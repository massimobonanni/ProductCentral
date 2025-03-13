using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace ProductCentral.LogisticCLI.Commands
{
    internal class UpdateProductStockQuantityCommand : CommandBase
    {
        public UpdateProductStockQuantityCommand(ServiceProvider serviceProvider = null) :
            base("stockqty", "Update the stock quantity for a product", serviceProvider)
        {
            var productIdOption = new Option<Guid>(
                name: "--productId",
                description: "The product id to update.")
            {
                IsRequired = true,
            };
            productIdOption.AddAlias("-id");
            AddOption(productIdOption);

            var stockQuantityOption = new Option<int>(
                name: "--stockQty",
                description: "The stock quantity to add to the product.")
            {
                IsRequired = true,
            };
            stockQuantityOption.AddAlias("-qty");
            AddOption(stockQuantityOption);

            this.SetHandler(CommandHandler, productIdOption, stockQuantityOption);
        }

        private Task CommandHandler(Guid productId, int stockQty)
        {
            if (!this._credentialManager.AreCredentialsValid())
            {
                Console.WriteLine("Please set credentials using the 'set' command.");
                return Task.CompletedTask;
            }

             var credentials= _credentialManager.GetCredentials();

            Console.WriteLine($"Product ID: {productId}");
            Console.WriteLine($"Stock Quantity: {stockQty}");

            return Task.CompletedTask;
        }
    }
}
