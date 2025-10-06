using Microsoft.Extensions.Configuration;
using static ProductCentral.CosmosDB.Configuration.CosmosDBConfiguration;

namespace ProductCentral.CosmosDB.Configuration;

/// <summary>
/// Configuration class for Azure Cosmos DB connection settings and authentication options.
/// Supports multiple authentication methods including connection string, service principal, and managed identity.
/// </summary>
internal class CosmosDBConfiguration
{
    /// <summary>
    /// Defines the available authentication types for connecting to Cosmos DB.
    /// </summary>
    internal enum AuthenticationTypes
    {
        /// <summary>
        /// Authentication using a connection string.
        /// </summary>
        ConnectionString,
        /// <summary>
        /// Authentication using Azure Service Principal with client ID, tenant ID, and client secret.
        /// </summary>
        ServicePrincipal,
        /// <summary>
        /// Authentication using Azure Managed Identity.
        /// </summary>
        ManagedIdentity
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosDBConfiguration"/> class.
    /// </summary>
    /// <param name="configuration">The configuration instance to read settings from.</param>
    public CosmosDBConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private readonly IConfiguration configuration;

    /// <summary>
    /// Gets or sets the Cosmos DB endpoint URL.
    /// </summary>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the Cosmos DB database name. Defaults to "productcentraldb".
    /// </summary>
    public string? DatabaseName { get; set; } = "productcentraldb";

    /// <summary>
    /// Gets or sets the Cosmos DB container name. Defaults to "products".
    /// </summary>
    public string? ContainerName { get; set; } = "products";

    /// <summary>
    /// Gets or sets the authentication type to use for Cosmos DB connection.
    /// </summary>
    public AuthenticationTypes AuthenticationType { get; set; }

    /// <summary>
    /// Gets or sets the Cosmos DB connection string for connection string authentication.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the Azure Service Principal client ID for service principal authentication.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the Azure tenant ID for service principal authentication.
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Gets or sets the Azure Service Principal client secret for service principal authentication.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Loads configuration values from the injected <see cref="IConfiguration"/> instance
    /// and determines the appropriate authentication type based on available settings.
    /// </summary>
    public void Load()
    {
        Endpoint = configuration["CosmosDB:Endpoint"];
        ClientId = configuration["CosmosDB:ClientId"];
        TenantId = configuration["CosmosDB:TenantId"];
        ClientSecret = configuration["CosmosDB:ClientSecret"];
        ConnectionString = configuration["CosmosDB:ConnectionString"];

        if (!string.IsNullOrEmpty(ConnectionString))
            AuthenticationType = AuthenticationTypes.ConnectionString;
        else if (!string.IsNullOrEmpty(ClientId))
            AuthenticationType = AuthenticationTypes.ServicePrincipal;
        else
            AuthenticationType = AuthenticationTypes.ManagedIdentity;

        if (configuration["CosmosDB:DatabaseName"] != null)
            DatabaseName = configuration["CosmosDB:DatabaseName"];
        if (configuration["CosmosDB:ContainerName"] != null)
            ContainerName = configuration["CosmosDB:ContainerName"];
    }
}

