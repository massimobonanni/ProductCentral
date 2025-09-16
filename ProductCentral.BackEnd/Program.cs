using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.EventGrid;
using Azure;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services
    .AddLogging()
    //.AddSingleton<ProductCentral.Core.Interfaces.IProductRepository, ProductCentral.Core.Implementations.InMemoryProductRepository>()
    .AddSingleton<ProductCentral.Core.Interfaces.IProductRepository, ProductCentral.CosmosDB.Implementations.CosmosDBProductRepository>()
    .AddSingleton<EventGridPublisherClient>(serviceProvider =>
     {
         var configuration = serviceProvider.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
         var endpoint = new Uri(configuration["EventGridTopicEndpoint"]!);
         var accessKey = new AzureKeyCredential(configuration["EventGridAccessKey"]!);
         return new EventGridPublisherClient(endpoint, accessKey);
     });

builder.Build().Run();
