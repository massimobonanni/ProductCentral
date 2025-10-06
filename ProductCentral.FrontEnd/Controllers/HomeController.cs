using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductCentral.FrontEnd.Models;

namespace ProductCentral.FrontEnd.Controllers;

/// <summary>
/// Controller responsible for handling home page requests and general application navigation.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for capturing application logs.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Displays the main home page of the application.
    /// </summary>
    /// <returns>The home page view.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays the privacy policy page.
    /// </summary>
    /// <returns>The privacy policy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Handles application errors and displays an error page with diagnostic information.
    /// </summary>
    /// <returns>The error view with error details including request ID for tracking.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
