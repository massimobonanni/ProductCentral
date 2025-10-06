using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCentral.LogisticCLI.Commands;

/// <summary>
/// Abstract base class for all CLI commands in the ProductCentral Logistic application.
/// Provides common functionality including dependency injection and credential management.
/// </summary>
internal abstract class CommandBase : Command
{
    /// <summary>
    /// The service provider for dependency injection.
    /// </summary>
    protected readonly ServiceProvider _serviceProvider;

    /// <summary>
    /// The credential manager for handling authentication credentials.
    /// </summary>
    protected readonly CredentialManager _credentialManager;

    /// <summary>
    /// Initializes a new instance of the CommandBase class.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public CommandBase(string name, string description, ServiceProvider serviceProvider) : base(name, description)
    {
        _serviceProvider = serviceProvider;
        _credentialManager = _serviceProvider.GetRequiredService<CredentialManager>();
    }
}
