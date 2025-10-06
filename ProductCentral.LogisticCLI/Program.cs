using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCentral.LogisticCLI;
using ProductCentral.LogisticCLI.Commands;
using ProductCentral.LogisticCLI.Utilities;
using System.CommandLine;

/// <summary>
/// Entry point for the ProductCentral Logistic CLI application.
/// Configures dependency injection, sets up available commands, and processes command line arguments.
/// </summary>
ConsoleUtility.WriteApplicationBanner();

/// <summary>
/// Configure dependency injection container with required services.
/// </summary>
var serviceCollection = new ServiceCollection();

/// <summary>
/// Register CredentialManager as a singleton service for managing authentication credentials.
/// The credentials are stored in an unencrypted file in the application directory.
/// </summary>
serviceCollection.TryAddSingleton<CredentialManager>(sp =>
{
    var rootPath = AppContext.BaseDirectory;
    var credentialManager = new CredentialManager(Path.Combine(rootPath, "credentials.dat"),
        null, encryptCredentials: false);
    return credentialManager;
});

/// <summary>
/// Build the service provider from the configured services.
/// </summary>
var serviceProvider = serviceCollection.BuildServiceProvider();

/// <summary>
/// Create the root command for the CLI application.
/// </summary>
var rootCommand = new RootCommand("Logistic console for ProductCentral");

/// <summary>
/// Register available CLI commands with the root command.
/// </summary>
rootCommand.AddCommand(new SetCredentialCommand(serviceProvider));
rootCommand.AddCommand(new UpdateProductStockQuantityCommand(serviceProvider));

/// <summary>
/// Execute the CLI application with the provided command line arguments.
/// </summary>
await rootCommand.InvokeAsync(args);
