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
        var productIdOption = new Option<Guid>("--productId")
        {
            Description = "The product id to update.",
            Required = true,
        };
        productIdOption.Aliases.Add("-id");
        Options.Add(productIdOption);

        var stockQuantityOption = new Option<int>("--stockQty")
        {
            Description = "The stock quantity to add to the product.",
            Required = true,
        };
        stockQuantityOption.Aliases.Add("-qty");
        Options.Add(stockQuantityOption);

        this.SetAction(async (ParseResult result, CancellationToken ct) =>
            await CommandHandler(result.GetValue(productIdOption), result.GetValue(stockQuantityOption), ct));
    }

    /// <summary>
    /// Handles the execution of the update stock quantity command.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to update.</param>
    /// <param name="stockQty">The stock quantity value to set for the product.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task CommandHandler(Guid productId, int stockQty, CancellationToken cancellationToken = default)
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
        await sender.SendMessageAsync(message, cancellationToken);

        Console.WriteLine("Message sent to Service Bus.");

        await sender.DisposeAsync();
        await serviceBusClient.DisposeAsync();
    }
}
