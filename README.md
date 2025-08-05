# MagiSystem

A decision support system inspired by the MAGI system from Neon Genesis Evangelion, featuring three AI sages with different personalities that collaborate through majority voting to provide balanced decision-making.

[日本語版はこちら](README.ja.md)

## Overview

MagiSystem leverages the collective intelligence of three AI agents (Sages), each with distinct thinking patterns, to support decision-making through democratic voting. Each AI analyzes the given topic from their unique perspective and casts a vote, with the final decision determined by majority rule.

### The Three Sages

- **🧠 Logic-type**: Makes decisions based on logical and objective analysis, emphasizing data and evidence
- **🛡️ Cautious-type**: Takes a conservative, risk-averse approach, prioritizing safety and stability  
- **❤️ Emotional-type**: Provides emotionally-driven and intuitive judgments, offering human-like perspectives and compromise solutions

## Quick Start

### Prerequisites

- .NET 8.0 SDK
- Azure OpenAI Service account with API access

### Using MagiService

The MagiSystem.Core library provides core services to integrate three-sage decision-making functionality into your applications.

#### 1. Add Project Reference

```bash
# Add project reference
dotnet add reference path/to/MagiSystem.Core

# Or use as NuGet package (to be published in the future)
# dotnet add package MagiSystem.Core
```

#### 2. Configure Azure OpenAI Client

```csharp
using Microsoft.Extensions.AI;
using MagiSystem.Core;

// Configure Azure OpenAI client
var chatClient = new AzureOpenAIClient(
    new Uri("https://your-resource.openai.azure.com/"),
    new AzureKeyCredential("your-api-key"))
    .AsChatClient("gpt-35-turbo");
```

#### 3. Create MagiService Instance

```csharp
// Create MagiService with default three sages
var magiService = new MagiService(chatClient);

// Or specify custom sages
var customSages = new List<Sage>
{
    new Sage("Data-driven logical judgment", chatClient),
    new Sage("Risk-management focused cautious judgment", chatClient),
    new Sage("User-experience focused emotional judgment", chatClient)
};
var magiService = new MagiService(chatClient, customSages);
```

#### 4. Execute Voting

```csharp
// Create vote option
var voteOption = new VoteOption(
    Topic: "Should we include new feature A in the next release?",
    YesCriteria: "Provides user value and is technically feasible",
    NoCriteria: "High risk and insufficient development resources"
);

// Execute voting by three sages
MagiResponse response = await magiService.MajorityVoteAsync(voteOption);

// Check results
Console.WriteLine($"Final Decision: {response.FinalDecision}");
Console.WriteLine($"Yes votes: {response.CountOfYes}, No votes: {response.CountOfNo}");

// Display reasons for each sage
Console.WriteLine("Reasons for Yes votes:");
foreach (var reason in response.YesReasons)
{
    Console.WriteLine($"- {reason}");
}

Console.WriteLine("Reasons for No votes:");
foreach (var reason in response.NoReasons)
{
    Console.WriteLine($"- {reason}");
}
```

### Sample Web Application

If you want to see the actual functionality in action, you can run the included web application:

```bash
git clone https://github.com/07JP27/MagiSystem.git
cd MagiSystem/MagiSystem.Web

# Configure Azure OpenAI settings in appsettings.json
dotnet run
```

Access `https://localhost:5001` in your browser to use the web interface.

## Customization

### Adding New Sage Personalities

You can customize the sage personalities by modifying the `MagiService` constructor in `MagiSystem.Core/MagiService.cs`:

```csharp
_sages = sages ?? new List<Sage>()
{
    new Sage("Your custom personality description", aiChatClient),
    new Sage("Another personality type", aiChatClient),
    new Sage("Third personality variant", aiChatClient)
};
```

### Extending the System

- **Custom AI Providers**: Implement your own `IChatClient` to use different AI services
- **Additional Sages**: Modify the system to support more than three sages
- **Custom Voting Logic**: Override the `DetermineFinalDecision` method for different decision algorithms
- **UI Customization**: Modify the Blazor components in `MagiSystem.Web/Components` to change the interface

### Configuration Options

The system supports various configuration options through `appsettings.json`:

```json
{
  "AzureOpenAI": {
    "Endpoint": "Your Azure OpenAI endpoint",
    "ApiKey": "Your API key", 
    "DeploymentName": "Your model deployment name"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Architecture

### Project Structure

- **MagiSystem.Core**: Core logic and domain models
  - `MagiService`: Main service orchestrating the voting process
  - `Sage`: Individual AI agent with personality-based decision making
  - Models: `VoteOption`, `MagiResponse`, `SageResponse`, etc.

- **MagiSystem.Web**: Blazor Server web application
  - Razor components for the user interface
  - Integration with the core services
  - Real-time voting results display

### Technology Stack

- **.NET 8**: Framework and runtime
- **Blazor Server**: Interactive web UI framework
- **Microsoft.Extensions.AI**: AI integration abstractions
- **Azure OpenAI**: AI language model provider
- **Bootstrap 5**: CSS framework for responsive UI

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Inspiration

This project is inspired by the MAGI system from the anime series "Neon Genesis Evangelion", where three supercomputers with different personality aspects collaborate to make critical decisions. While our implementation uses AI language models instead of supercomputers, the core concept of diverse perspectives contributing to collective decision-making remains the same.