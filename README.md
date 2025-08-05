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

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/07JP27/MagiSystem.git
   cd MagiSystem
   ```

2. **Configure Azure OpenAI settings**
   
   Edit `MagiSystem.Web/appsettings.json` or set environment variables:
   ```json
   {
     "AzureOpenAI": {
       "Endpoint": "https://your-resource.openai.azure.com/",
       "ApiKey": "your-api-key",
       "DeploymentName": "gpt-35-turbo"
     }
   }
   ```

   Or use environment variables:
   ```bash
   export AzureOpenAI__Endpoint="https://your-resource.openai.azure.com/"
   export AzureOpenAI__ApiKey="your-api-key"
   export AzureOpenAI__DeploymentName="gpt-35-turbo"
   ```

3. **Build and run**
   ```bash
   dotnet build
   cd MagiSystem.Web
   dotnet run
   ```

4. **Access the application**
   
   Open your browser and navigate to `https://localhost:5001` (or the URL shown in the terminal).

### Usage

1. **Enter your topic**: Describe the decision you need to make
2. **Set voting criteria**: Define what constitutes a "Yes" and "No" vote
3. **Execute voting**: The three MAGI will analyze and vote on your topic
4. **Review results**: See the majority decision along with detailed reasoning from each sage

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