using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace ProductCentral.LogisticCLI.Commands
{
    internal class SetCredentialCommand : CommandBase
    {
        public SetCredentialCommand(ServiceProvider serviceProvider = null) :
            base("set", "Set the credential to access to the Service Bus", serviceProvider)
        {
            var endpointOption = new Option<string>(
                name: "--endpoint",
                description: "The endpoint of Azure Service Bus resource.")
            {
                IsRequired = true,
            };
            endpointOption.AddAlias("-e");
            AddOption(endpointOption);

            var keyOption = new Option<string>(
                name: "--key",
                description: "The key of Azure Service Bus resource.")
            {
                IsRequired = true,
            };
            keyOption.AddAlias("-k");
            AddOption(keyOption);

            this.SetHandler(CommandHandler, endpointOption, keyOption);
        }

        private Task CommandHandler(string endpoint, string key)
        {
            var credentials = new Credentials
            {
                Url = endpoint,
                Key = key
            };

            this._credentialManager.SetupCredentials(credentials);

            Console.WriteLine("Credentials have been set successfully.");

            return Task.CompletedTask;
        }
    }
}
