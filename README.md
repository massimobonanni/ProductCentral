# ProductCentral

ProductCentral is a sample solution that demonstrates a small product management platform built with modern .NET technologies. It includes:

- An Azure Functions-based BackEnd that hosts product-related HTTP APIs and listens to Service Bus messages.
- An ASP.NET Core FrontEnd (Razor Pages / MVC) that consumes the backend APIs.
- A lightweight CLI tool for logistic operations (stock updates, credential management).
- A RestClient library that encapsulates calls to the backend HTTP APIs.
- Messaging types to integrate with Azure Service Bus and an in-memory product repository for samples and tests.
- Bicep templates for infrastructure-as-code under `ProductCentral.IaC`.

This repository is intended for learning, demos, and local development. It is not production hardened.

## Projects in this solution

- `ProductCentral.BackEnd` — Azure Functions (isolated worker) exposing REST endpoints:
  - POST /api/products — Add a product
  - GET /api/products — Search products (optional `searchTerm` query)
  - GET /api/products/{productId} — Get product by ID
  - PUT /api/products/{productId}/update-stock — Update stock quantity
  - Functions also listen to a Service Bus Topic subscription to update stock from messages.

- `ProductCentral.FrontEnd` — ASP.NET Core web app that calls the backend via the `ProductCentral.RestClient`.
- `ProductCentral.Core` — Domain entities and an in-memory repository used by the backend for demo purposes.
- `ProductCentral.RestClient` — A simple typed HTTP client (`ProductApiClient`) used both by the front end and CLI. It expects the Azure Functions key in a `code` query string parameter.
- `ProductCentral.LogisticCLI` — A console app with commands to set credentials and send stock update requests.
- `ProductCentral.Messaging` — Message DTOs used for Service Bus integration.

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022/2023 or VS Code
- Azure Functions Core Tools (for running functions locally)
- Azure CLI (az) if you plan to deploy to Azure
- (Optional) azd (Azure Developer CLI) to simplify cloud provisioning
- (Optional) Azurite for local Azure Storage emulation (or an Azure storage account)

## Local development

1. Restore and build
   - From the solution root run:
     dotnet restore; dotnet build

2. Run the BackEnd (Azure Functions) locally
   - From `ProductCentral.BackEnd` folder run the Functions host. If you have Azure Functions Core Tools installed:
     func start
   - The included Test HTTP files assume the functions host runs at `http://localhost:7023` (see `ProductCentral.BackEnd/Tests/` files). If your Functions host runs on a different port, update the test requests accordingly.
   - The functions use `local.settings.json` for local configuration (Service Bus connection string uses a placeholder value; replace for real integrations or use the Service Bus emulator you prefer).

3. Run the FrontEnd
   - From `ProductCentral.FrontEnd` run:
     dotnet run
   - Configure the FrontEnd to point at the local Functions host if needed (see `appsettings.Development.json` and/or user secrets in the FrontEnd project).

4. Use the RestClient
   - `ProductCentral.RestClient` exposes `ProductApiClient`. Example usage:
     - Construct with: HttpClient, base Uri (e.g. `http://localhost:7023/api/`), an Azure Function key (or empty string for local anon if enabled), and an ILogger.
     - Methods:
       - AddProductAsync(AddProductRequest)
       - GetProductByIdAsync(Guid)
       - SearchProductsAsync(string)

   - For quick manual testing use the HTTP files under `ProductCentral.BackEnd/Tests/*.http` with VS Code REST Client or similar tools.

5. Use the CLI
   - The logistic CLI provides two commands (see `ProductCentral.LogisticCLI/Commands`):
     - `set-credential` — stores credentials used by the CLI
     - `update-product-stock` — posts a stock update to the backend API (or publishes to Service Bus depending on implementation)
   - Example: from the CLI project folder run:
     dotnet run -- set-credential --help

## Seeding data and tests

- The `ProductCentral.BackEnd/Tests/SeedProducts.http` file contains a set of HTTP POST requests that add sample products to the local backend. Use the REST client extension or curl to run them.
- Unit tests are not included in this sample; the in-memory repository is used for quick manual validation.

## Environment and configuration

- Local function settings are stored in `ProductCentral.BackEnd/local.settings.json`. Do not commit secrets to source control. When running locally you can override these settings via environment variables or user secrets for the FrontEnd project.

- The RestClient constructs API URLs and appends the Function key as a `code` query string parameter when an API key is provided.

## Deploying to Azure (overview)

This repository includes Bicep templates in `ProductCentral.IaC` suitable for provisioning the backend and frontend resources. The repository also includes an Azure Functions app and a web app project.

- Using Azure CLI + Bicep:
  - Build or validate the Bicep templates in `ProductCentral.IaC`, then deploy using `az deployment group create` or `az deployment sub create` depending on the target scope.
  - Ensure you supply necessary parameters such as resource group, names, and secrets (Function keys, Service Bus connection details, etc.).

Security note: the sample `local.settings.json` contains a Service Bus connection string placeholder. Replace secrets with Key Vault references for production deployments and follow the Principle of Least Privilege.

If you want step-by-step deployment assistance for this repository (recommended), open an issue or request a deployment guide and include the subscription and desired naming conventions — we can provide exact `az`/`azd` commands to provision resources.

## Contributing

Contributions are welcome. Typical ways to contribute:

- Open issues for bugs or feature requests
- Fork the repo and submit pull requests
- Improve tests and add CI workflow (GitHub Actions)

Please follow standard .NET code style and include unit tests for new functionality.

## Troubleshooting

- If the Functions host fails to start, ensure Azure Functions Core Tools is installed and that the runtime (`FUNCTIONS_WORKER_RUNTIME`) is set to `dotnet-isolated`.
- If Service Bus integration is required locally and you do not have a live Service Bus, either use an emulator or remove/disable Service Bus-triggered functions by commenting out the ServiceBusTrigger attribute while testing.

## License

This project is provided as-is for learning purposes. Add a proper license file if you intend to publish or reuse this code in other projects.