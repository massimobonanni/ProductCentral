using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProductCentral.Core.Entities;
using ProductCentral.Core.Interfaces;
using ProductCentral.Core.Responses;

namespace ProductCentral.Core.Implementations;

/// <summary>
/// In-memory implementation of the <see cref="IProductRepository"/> interface.
/// </summary>
public class InMemoryProductRepository : IProductRepository
{
    /// <summary>
    /// Static dictionary to store products in memory.
    /// </summary>
    private static readonly Dictionary<Guid, Product> _products = new();

    /// <summary>
    /// Adds a new product to the repository.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<ServiceResponse> AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        if (_products.ContainsKey(product.Id))
        {
            return Task.FromResult(new ServiceResponse { Success = false, ErrorMessage = "Product already exists." });
        }

        _products[product.Id] = product;
        return Task.FromResult(new ServiceResponse { Success = true });
    }

    /// <summary>
    /// Updates the details of an existing product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="title">The new title of the product.</param>
    /// <param name="description">The new description of the product.</param>
    /// <param name="unitPrice">The new unit price of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<ServiceResponse> UpdateProductDetailsAsync(Guid productId, string title, string description, decimal unitPrice, CancellationToken cancellationToken)
    {
        if (!_products.TryGetValue(productId, out var product))
        {
            return Task.FromResult(new ServiceResponse { Success = false, ErrorMessage = "Product not found." });
        }

        product.Title = title;
        product.Description = description;
        product.UnitPrice = unitPrice;
        return Task.FromResult(new ServiceResponse { Success = true });
    }

    /// <summary>
    /// Updates the stock quantity of an existing product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="stockQuantity">The new stock quantity of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<ServiceResponse> UpdateStockQuantityAsync(Guid productId, int stockQuantity, CancellationToken cancellationToken)
    {
        if (!_products.TryGetValue(productId, out var product))
        {
            return Task.FromResult(new ServiceResponse { Success = false, ErrorMessage = "Product not found." });
        }

        product.StockQuantity = stockQuantity;
        return Task.FromResult(new ServiceResponse { Success = true });
    }

    /// <summary>
    /// Deletes a product from the repository.
    /// </summary>
    /// <param name="productId">The ID of the product to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<ServiceResponse> DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        if (!_products.Remove(productId))
        {
            return Task.FromResult(new ServiceResponse { Success = false, ErrorMessage = "Product not found." });
        }

        return Task.FromResult(new ServiceResponse { Success = true });
    }

    /// <summary>
    /// Retrieves a specific product by ID.
    /// </summary>
    /// <param name="productId">The ID of the product to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with the product as the result.</returns>
    public Task<ServiceResponse<Product>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        if (!_products.TryGetValue(productId, out var product))
        {
            return Task.FromResult(new ServiceResponse<Product> { Success = false, ErrorMessage = "Product not found." });
        }

        return Task.FromResult(new ServiceResponse<Product> { Success = true, Result = product });
    }

    /// <summary>
    /// Searches for products by title and description.
    /// </summary>
    /// <param name="searchTerm">The term to search for in the title and description.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of matching products as the result.</returns>
    public Task<ServiceResponse<IEnumerable<Product>>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var results = _products.Values
            .Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(new ServiceResponse<IEnumerable<Product>> { Success = true, Result = results });
    }
}
