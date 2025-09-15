using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCentral.Core.Interfaces;
using ProductCentral.Core.Responses;
using ProductCentral.CosmosDB.Configuration;
using ProductCentral.CosmosDB.Entities;
using System.Net;

namespace ProductCentral.CosmosDB.Implementations;

/// <summary>
/// CosmosDB implementation of the product repository.
/// </summary>
public class CosmosDBProductRepository : IProductRepository
{
    private readonly CosmosDBConfiguration _configuration;
    private readonly ILogger<CosmosDBProductRepository> _logger;

    public CosmosDBProductRepository(IConfiguration configuration,
        ILogger<CosmosDBProductRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = new CosmosDBConfiguration(configuration);
        _configuration.Load();

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private CosmosClient CreateCosmosDBClient()
    {
        CosmosClient cosmosClient = null;

        switch (_configuration.AuthenticationType)
        {
            case CosmosDBConfiguration.AuthenticationTypes.ConnectionString:
                cosmosClient = new CosmosClient(_configuration.ConnectionString);
                break;
            case CosmosDBConfiguration.AuthenticationTypes.ServicePrincipal:
                var servicePrincipalCredential = new ClientSecretCredential(
                    _configuration.TenantId, _configuration.ClientId, _configuration.ClientSecret);
                cosmosClient = new CosmosClient(_configuration.Endpoint,
                    servicePrincipalCredential);
                break;
            case CosmosDBConfiguration.AuthenticationTypes.ManagedIdentity:
                var managedIdentityCredential = new DefaultAzureCredential();
                cosmosClient = new CosmosClient(_configuration.Endpoint,
                    managedIdentityCredential);
                break;
            default:
                throw new InvalidOperationException("Unsupported authentication type.");
        }
        return cosmosClient;
    }


    /// <summary>
    /// Adds a new product to the repository.
    /// </summary>
    public async Task<ServiceResponse> AddProductAsync(Core.Entities.Product product, CancellationToken cancellationToken)
    {
        if (product == null)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = "Product cannot be null."
            };
        }

        using var cosmosClient = CreateCosmosDBClient();

        try
        {
            var container = cosmosClient.GetContainer(_configuration.DatabaseName, _configuration.ContainerName);

            var dbProduct = new Product(product);

            var response = await container.CreateItemAsync(
                dbProduct,
                new PartitionKey(product.Id.ToString()),
                cancellationToken: cancellationToken);

            return new ServiceResponse { Success = true };
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = "Product with the same ID already exists."
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = $"An error occurred while adding the product: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Updates the details of an existing product.
    /// </summary>
    public async Task<ServiceResponse> UpdateProductDetailsAsync(Guid productId, string title, string description, decimal unitPrice, CancellationToken cancellationToken)
    {
        using var cosmosClient = CreateCosmosDBClient();

        try
        {
            var container = cosmosClient.GetContainer(_configuration.DatabaseName, _configuration.ContainerName);

            var readProductResponse = await container.ReadItemAsync<Product>(
                productId.ToString(),
                new PartitionKey(productId.ToString()),
                cancellationToken: cancellationToken);

            var product = readProductResponse.Resource;
            product.Title = title;
            product.Description = description;
            product.UnitPrice = unitPrice;

            await container.ReplaceItemAsync(
                product,
                product.Id.ToString(),
                new PartitionKey(product.Id.ToString()),
                cancellationToken: cancellationToken);

            return new ServiceResponse { Success = true };
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = "Product not found."
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = $"An error occurred while updating the product: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Updates the stock quantity of an existing product.
    /// </summary>
    public async Task<ServiceResponse> UpdateStockQuantityAsync(Guid productId, int stockQuantity, CancellationToken cancellationToken)
    {
        using var cosmosClient = CreateCosmosDBClient();

        try
        {
            var container = cosmosClient.GetContainer(_configuration.DatabaseName, _configuration.ContainerName);

            var readProductResponse = await container.ReadItemAsync<Product>(
                productId.ToString(),
                new PartitionKey(productId.ToString()),
                cancellationToken: cancellationToken);

            var product = readProductResponse.Resource;
            product.StockQuantity = stockQuantity;

            await container.ReplaceItemAsync(
                product,
                product.Id.ToString(),
                new PartitionKey(product.Id.ToString()),
                cancellationToken: cancellationToken);

            return new ServiceResponse { Success = true };
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = "Product not found."
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = $"An error occurred while updating the stock quantity: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Deletes a product from the repository.
    /// </summary>
    public async Task<ServiceResponse> DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        using var cosmosClient = CreateCosmosDBClient();

        try
        {
            var container = cosmosClient.GetContainer(_configuration.DatabaseName, _configuration.ContainerName);

            await container.DeleteItemAsync<Product>(
                productId.ToString(),
                new PartitionKey(productId.ToString()),
                cancellationToken: cancellationToken);

            return new ServiceResponse { Success = true };
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = "Product not found."
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Success = false,
                ErrorMessage = $"An error occurred while deleting the product: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Retrieves a specific product by ID.
    /// </summary>
    public async Task<ServiceResponse<Core.Entities.Product>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        using var cosmosClient = CreateCosmosDBClient();

        try
        {
            var container = cosmosClient.GetContainer(_configuration.DatabaseName, _configuration.ContainerName);

            var response = await container.ReadItemAsync<Product>(
                productId.ToString(),
                new PartitionKey(productId.ToString()),
                cancellationToken: cancellationToken);

            return new ServiceResponse<Core.Entities.Product>
            {
                Success = true,
                Result = response.Resource.ToCoreEntity()
            };
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new ServiceResponse<Core.Entities.Product>
            {
                Success = false,
                ErrorMessage = "Product not found."
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Core.Entities.Product>
            {
                Success = false,
                ErrorMessage = $"An error occurred while retrieving the product: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Searches for products by title and description.
    /// </summary>
    public async Task<ServiceResponse<IEnumerable<Core.Entities.Product>>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken)
    {
        using var cosmosClient = CreateCosmosDBClient();

        try
        {
            var container = cosmosClient.GetContainer(_configuration.DatabaseName, _configuration.ContainerName);

            QueryDefinition query = null;

            if (string.IsNullOrWhiteSpace(searchTerm))
                query = new QueryDefinition("SELECT * FROM c");
            else
                query = new QueryDefinition(
                    "SELECT * FROM c WHERE CONTAINS(UPPER(c.Title), UPPER(@searchTerm)) OR CONTAINS(UPPER(c.Description), UPPER(@searchTerm))")
                    .WithParameter("@searchTerm", searchTerm);

            var iterator = container.GetItemQueryIterator<Product>(query);
            var products = new List<Core.Entities.Product>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                if (response.Any())
                    products.AddRange(response.Select(p => p.ToCoreEntity()).ToList());
            }

            return new ServiceResponse<IEnumerable<Core.Entities.Product>>
            {
                Success = true,
                Result = products
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<IEnumerable<Core.Entities.Product>>
            {
                Success = false,
                ErrorMessage = $"An error occurred while searching for products: {ex.Message}"
            };
        }
    }
}