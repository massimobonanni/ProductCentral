using Microsoft.Extensions.Configuration;
using static ProductCentral.CosmosDB.Configuration.CosmosDBConfiguration;

namespace ProductCentral.CosmosDB.Configuration;

internal class CosmosDBConfiguration
{
    internal enum AuthenticationTypes
    {
        ConnectionString,
        ServicePrincipal,
        ManagedIdentity
    }

    public CosmosDBConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private readonly IConfiguration configuration;

    public string? Endpoint { get; set; }
    public string? DatabaseName { get; set; } = "productcentraldb";
    public string? ContainerName { get; set; } = "products";

    public AuthenticationTypes AuthenticationType { get; set; }

    public string? ConnectionString { get; set; }

    public string? ClientId { get; set; }
    public string? TenantId { get; set; }
    public string? ClientSecret { get; set; }

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

