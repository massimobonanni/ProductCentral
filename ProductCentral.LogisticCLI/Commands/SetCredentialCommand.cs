using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace ProductCentral.LogisticCLI.Commands
{
    internal class SetCredentialCommand : CommandBase
    {
        public SetCredentialCommand(ServiceProvider serviceProvider = null) :
            base("set", "Set the connectionstring to access to the Service Bus", serviceProvider)
        {
            var connectionStringOption = new Option<string>(
                name: "--connString",
                description: "The connection string to access to Azure Service Bus resource.")
            {
                IsRequired = true,
            };
            connectionStringOption.AddAlias("-cs");
            AddOption(connectionStringOption);

            var destinationOption = new Option<string>(
                name: "--dest",
                description: "The topic or queue name of the Azure Service Bus resource.")
            {
                IsRequired = true,
            };
            destinationOption.AddAlias("-d");
            AddOption(destinationOption);

            this.SetHandler(CommandHandler, connectionStringOption, destinationOption);
        }

        private Task CommandHandler(string connectionString,string destination)
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
}
