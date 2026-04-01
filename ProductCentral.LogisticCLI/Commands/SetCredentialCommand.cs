using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace ProductCentral.LogisticCLI.Commands;

/// <summary>
/// Command for setting Azure Service Bus credentials including connection string and destination.
/// </summary>
internal class SetCredentialCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetCredentialCommand"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection. Optional parameter.</param>
    public SetCredentialCommand(ServiceProvider serviceProvider = null) :
        base("set", "Set the connectionstring to access to the Service Bus", serviceProvider)
    {
        var connectionStringOption = new Option<string>("--connString")
        {
            Description = "The connection string to access to Azure Service Bus resource.",
            Required = true,
        };
        connectionStringOption.Aliases.Add("-cs");
        Options.Add(connectionStringOption);

        var destinationOption = new Option<string>("--dest")
        {
            Description = "The topic or queue name of the Azure Service Bus resource.",
            Required = true,
        };
        destinationOption.Aliases.Add("-d");
        Options.Add(destinationOption);

        this.SetAction((ParseResult result) =>
            CommandHandler(result.GetValue(connectionStringOption)!, result.GetValue(destinationOption)!));
    }

    /// <summary>
    /// Handles the command execution by setting up the Azure Service Bus credentials.
    /// </summary>
    /// <param name="connectionString">The connection string for Azure Service Bus.</param>
    /// <param name="destination">The topic or queue name for the Azure Service Bus resource.</param>
    /// <returns>A completed task.</returns>
    private Task CommandHandler(string connectionString, string destination)
    {
        var credentials = new Credentials
        {
            ConnectionString = connectionString,
            TopicOrQueueName = destination,
        };

        this._credentialManager.SetupCredentials(credentials);

        Console.WriteLine("Credentials have been set successfully.");

        return Task.CompletedTask;
    }
}
