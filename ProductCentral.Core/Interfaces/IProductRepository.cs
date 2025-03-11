using ProductCentral.Core.Entities;
using ProductCentral.Core.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCentral.Core.Interfaces;

/// <summary>
/// Interface for managing products in a repository.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Adds a new product to the repository.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ServiceResponse> AddProductAsync(Product product, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the details of an existing product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="title">The new title of the product.</param>
    /// <param name="description">The new description of the product.</param>
    /// <param name="unitPrice">The new unit price of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ServiceResponse> UpdateProductDetailsAsync(Guid productId, string title, string description, decimal unitPrice, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the stock quantity of an existing product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="stockQuantity">The new stock quantity of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ServiceResponse> UpdateStockQuantityAsync(Guid productId, int stockQuantity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a product from the repository.
    /// </summary>
    /// <param name="productId">The ID of the product to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ServiceResponse> DeleteProductAsync(Guid productId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a specific product by ID.
    /// </summary>
    /// <param name="productId">The ID of the product to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with the product as the result.</returns>
    Task<ServiceResponse<Product>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for products by title and description.
    /// </summary>
    /// <param name="searchTerm">The term to search for in the title and description.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of matching products as the result.</returns>
    Task<ServiceResponse<IEnumerable<Product>>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken);
}
