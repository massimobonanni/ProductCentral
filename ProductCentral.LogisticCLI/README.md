# ProductCentral Logistic CLI

A command-line interface for managing ProductCentral logistics operations, built with .NET 8 and System.CommandLine.

## Overview

The Logistic Console provides a set of commands to interact with the ProductCentral system, specifically for managing product stock quantities through Azure Service Bus messaging.

## Installation & Build

1. Navigate to the project directory:
   ```bash      
   cd ProductCentral.LogisticCLI
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the CLI:
   ```bash
   dotnet run
   ```

   Or after building:
   ```bash
   ./bin/Debug/net8.0/logistic.exe
   ```

## Prerequisites

- .NET 8 SDK
- Azure Service Bus connection (for message sending functionality)

## Configuration

Before using the CLI commands that interact with Azure Service Bus, you must configure your credentials using the `set` command.

## Available Commands

### `set` - Set Credentials
Configures the Azure Service Bus connection string and destination for the CLI.

**Usage:**
```bash
logistic set --connString <connection-string> --dest <topic-or-queue-name>
```

**Parameters:**
- `--connString`, `-cs` (required): The connection string to access the Azure Service Bus resource
- `--dest`, `-d` (required): The topic or queue name of the Azure Service Bus resource

**Example:**
```bash
logistic set -cs "Endpoint=sb://your-servicebus.servicebus.windows.net/;SharedAccessKeyName=..." -d "product-updates"
```

### `stockqty` - Update Product Stock Quantity
Sends a message to update the stock quantity for a specific product via Azure Service Bus.

**Usage:**
```bash
logistic stockqty --productId <product-guid> --stockQty <quantity>
```

**Parameters:**
- `--productId`, `-id` (required): The GUID of the product to update
- `--stockQty`, `-qty` (required): The stock quantity to add to the product (integer value)

**Example:**
```bash
logistic stockqty -id 12345678-1234-1234-1234-123456789012 -qty 100
```

**Note:** You must set credentials using the `set` command before using this command.

## Command Examples

1. **Configure credentials:**
   ```bash
   logistic set --connString "Endpoint=sb://productcentral.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=your-key" --dest "stock-updates"
   ```

2. **Update product stock:**
   ```bash
   logistic stockqty --productId a1b2c3d4-e5f6-7890-abcd-ef1234567890 --stockQty 50
   ```

## Help

To see available commands and options:
```bash
logistic --help
```

To get help for a specific command:
```bash
logistic set --help
logistic stockqty --help
```

## Architecture

The CLI uses:
- **System.CommandLine** for command-line parsing and handling
- **Azure.Messaging.ServiceBus** for Azure Service Bus integration
- **Microsoft.Extensions.DependencyInjection** for dependency injection
- **Figgle** for ASCII art banner display

## Project Structure

```
ProductCentral.LogisticCLI/
├── Commands/
│   ├── CommandBase.cs              # Base class for all commands
│   ├── SetCredentialCommand.cs     # Credential configuration command
│   └── UpdateProductStockQuantityCommand.cs # Stock update command
├── Utilities/
│   └── ConsoleUtility.cs          # Console output utilities
├── CredentialManager.cs           # Manages stored credentials
└── Program.cs                     # Application entry point
```

## Dependencies

- ProductCentral.Core
- ProductCentral.Messaging
- Azure.Messaging.ServiceBus (v7.18.4)
- Figgle (v0.5.1)
- Microsoft.Extensions.DependencyInjection (v8.0.1)
- System.CommandLine (v2.0.0-beta4.22272.1)

## Credential Storage

Credentials are stored locally in a `credentials.dat` file in the application directory. The current implementation stores credentials in plain text (encryption is disabled).

## Error Handling

- If credentials are not set, the `stockqty` command will display: "Please set credentials using the 'set' command."
- Invalid command syntax will show help information
- Missing required parameters will display appropriate error messages