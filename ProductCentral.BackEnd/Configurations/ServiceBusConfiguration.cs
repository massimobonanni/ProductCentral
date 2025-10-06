using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring Azure Service Bus services in the dependency injection container.
/// </summary>
public static class ServiceBusConfiguration
{
    /// <summary>
    /// Adds Azure Service Bus client services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance containing application configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method configures the Azure Service Bus client using the connection string from the "ServiceBusConnectionString" configuration key.
    /// </remarks>
    public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(configuration["ServiceBusConnectionString"]);
        });

        return services;
    }
}
