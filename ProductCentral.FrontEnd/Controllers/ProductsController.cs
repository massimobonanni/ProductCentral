using Microsoft.AspNetCore.Mvc;
using ProductCentral.Core.Entities;
using ProductCentral.FrontEnd.Models.ProductsController;
using ProductCentral.RestClient;

namespace ProductCentral.FrontEnd.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductApiClient productApiClient;

        public ProductsController(ILogger<ProductsController> logger, ProductApiClient productApiClient)
        {
            _logger = logger;
            this.productApiClient = productApiClient;
        }

        // GET: ProductsController
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

        // GET: ProductsController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var model= new DetailsViewModel();
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

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
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

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
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

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
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
}
