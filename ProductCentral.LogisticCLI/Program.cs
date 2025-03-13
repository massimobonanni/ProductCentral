using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCentral.LogisticCLI;
using ProductCentral.LogisticCLI.Commands;
using ProductCentral.LogisticCLI.Utilities;
using System.CommandLine;

ConsoleUtility.WriteApplicationBanner();

var serviceCollection = new ServiceCollection();
serviceCollection.TryAddSingleton<CredentialManager>(sp =>
{
    var rootPath = AppContext.BaseDirectory;
    var credentialManager = new CredentialManager(Path.Combine(rootPath, "credentials.dat"),
        null, encryptCredentials: false);
    return credentialManager;
});

// Build the service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

var rootCommand = new RootCommand("Logistic console for ProductCentral");

rootCommand.AddCommand(new SetCredentialCommand(serviceProvider));
rootCommand.AddCommand(new UpdateProductStockQuantityCommand(serviceProvider));

await rootCommand.InvokeAsync(args);
