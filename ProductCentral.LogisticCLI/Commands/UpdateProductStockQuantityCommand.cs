using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using ProductCentral.Messaging.Messages;
using System.CommandLine;
using System.Text.Json;

namespace ProductCentral.LogisticCLI.Commands;

/// <summary>
/// Command for updating the stock quantity of a product by sending a message to Service Bus.
/// </summary>
internal class UpdateProductStockQuantityCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductStockQuantityCommand"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection. Can be null.</param>
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

    /// <summary>
    /// Handles the execution of the update stock quantity command.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to update.</param>
    /// <param name="stockQty">The stock quantity value to set for the product.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task CommandHandler(Guid productId, int stockQty)
    {
        if (!this._credentialManager.AreCredentialsValid())
        {
            Console.WriteLine("Please set credentials using the 'set' command.");
            return;
        }

        var credentials = _credentialManager.GetCredentials();

        Console.WriteLine($"Product ID: {productId}");
        Console.WriteLine($"Stock Quantity: {stockQty}");

        var updateStockPayload = new UpdateProductStockQuantityMessage()
        {
            ProductId = productId,
            StockQuantity = stockQty
        };

        // Create a Service Bus client
        var serviceBusClient = new ServiceBusClient(credentials.ConnectionString);
        var sender = serviceBusClient.CreateSender(credentials.TopicOrQueueName);

        // Create a message to send
        var message = new ServiceBusMessage(JsonSerializer.Serialize(updateStockPayload));

        // Send the message
        await sender.SendMessageAsync(message);

        Console.WriteLine("Message sent to Service Bus.");

        await sender.DisposeAsync();
        await serviceBusClient.DisposeAsync();
    }
}
