using Microsoft.AspNetCore.Mvc;
using ProductCentral.Core.Entities;
using ProductCentral.FrontEnd.Models.ProductsController;
using ProductCentral.RestClient;

namespace ProductCentral.FrontEnd.Controllers;

/// <summary>
/// Controller for managing product-related operations in the front-end application.
/// Handles CRUD operations for products through interaction with the ProductApiClient.
/// </summary>
public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ProductApiClient productApiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="logger">Logger instance for recording controller operations.</param>
    /// <param name="productApiClient">Client for making API calls to the product service.</param>
    public ProductsController(ILogger<ProductsController> logger, ProductApiClient productApiClient)
    {
        _logger = logger;
        this.productApiClient = productApiClient;
    }

    /// <summary>
    /// Displays the main products listing page with all available products.
    /// </summary>
    /// <returns>A view containing the list of products ordered by title, or error information if the request fails.</returns>
    public async Task<ActionResult> Index()
    {
        var model = new IndexViewModel();

        try
        {
            var searchResponse = await productApiClient.SearchProductsAsync();
            model.Products = searchResponse.Products.OrderBy(p => p.Title).ToList();
            model.SearchTerm = null;
        }
        catch (Exception ex)
        {
            model.HasErrror = true;
            model.ErrorMessage = ex.Message;
        }

        return View(model);
    }

    /// <summary>
    /// Displays detailed information for a specific product.
    /// </summary>
    /// <param name="id">The unique identifier of the product to display.</param>
    /// <returns>A view containing the product details, or error information if the product is not found or request fails.</returns>
    public async Task<ActionResult> Details(Guid id)
    {
        var model = new DetailsViewModel();
        try
        {
            var getByIdResponse = await productApiClient.GetProductByIdAsync(id);

            model.Product = getByIdResponse.Product;
        }
        catch (Exception ex)
        {
            model.HasErrror = true;
            model.ErrorMessage = ex.Message;
        }

        return View(model);
    }

    /// <summary>
    /// Displays the form for creating a new product.
    /// </summary>
    /// <returns>A view containing the product creation form.</returns>
    public ActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles the submission of the product creation form.
    /// </summary>
    /// <param name="collection">The form data collection containing the new product information.</param>
    /// <returns>Redirects to the Index action on success, or returns the Create view on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    /// <summary>
    /// Displays the form for editing an existing product.
    /// </summary>
    /// <param name="id">The unique identifier of the product to edit.</param>
    /// <returns>A view containing the product edit form.</returns>
    public ActionResult Edit(int id)
    {
        return View();
    }

    /// <summary>
    /// Handles the submission of the product edit form.
    /// </summary>
    /// <param name="id">The unique identifier of the product being edited.</param>
    /// <param name="collection">The form data collection containing the updated product information.</param>
    /// <returns>Redirects to the Index action on success, or returns the Edit view on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    /// <summary>
    /// Displays the confirmation page for deleting a product.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>A view containing the product deletion confirmation form.</returns>
    public ActionResult Delete(int id)
    {
        return View();
    }

    /// <summary>
    /// Handles the confirmation of product deletion.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <param name="collection">The form data collection from the deletion confirmation.</param>
    /// <returns>Redirects to the Index action on success, or returns the Delete view on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
