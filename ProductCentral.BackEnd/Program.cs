using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure;

/// <summary>
/// Entry point for the ProductCentral BackEnd Azure Functions application.
/// Configures dependency injection, logging, and event publishing services.
/// </summary>
var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

// Configure services for dependency injection
builder.Services
    .AddLogging()
    // Use CosmosDB implementation for production, InMemory for development/testing
    //.AddSingleton<ProductCentral.Core.Interfaces.IProductRepository, ProductCentral.Core.Implementations.InMemoryProductRepository>()
    .AddSingleton<ProductCentral.Core.Interfaces.IProductRepository, ProductCentral.CosmosDB.Implementations.CosmosDBProductRepository>()
    // Configure EventGrid client for publishing events
    .AddSingleton<EventGridPublisherClient>(serviceProvider =>
     {
         var configuration = serviceProvider.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
         var endpoint = new Uri(configuration["EventGridTopicEndpoint"]!);
         var accessKey = new AzureKeyCredential(configuration["EventGridAccessKey"]!);
         return new EventGridPublisherClient(endpoint, accessKey);
     });

builder.Build().Run();
