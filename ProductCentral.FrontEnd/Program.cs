using ProductCentral.RestClient;

/// <summary>
/// Entry point for the ProductCentral FrontEnd ASP.NET Core Razor Pages application.
/// Configures services, middleware pipeline, and routing for the web application.
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient for making HTTP requests
builder.Services.AddHttpClient();

// Register ProductApiClient as a transient service with configuration
builder.Services.AddTransient<ProductApiClient>(sp =>
{
    var httpClient = sp.GetService<HttpClient>();
    var logger = sp.GetService<ILogger<ProductApiClient>>();
    var apiKey = builder.Configuration["ProductApiKey"];
    var baseUrl = builder.Configuration["ProductApiUrl"];
    return new ProductApiClient(httpClient, new Uri(baseUrl), apiKey, logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use exception handler for production environments
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure default route for MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
