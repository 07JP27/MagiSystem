using Microsoft.Extensions.AI;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MagiSystem.Core;

namespace MagiSystem.Web.Services;

// Mock response wrapper that matches the expected interface
public class MockAIResponse<T>
{
    public ChatFinishReason FinishReason { get; set; } = ChatFinishReason.Stop;
    public string Text { get; set; } = "";
    private readonly T? _result;

    public MockAIResponse(T result)
    {
        _result = result;
        Text = System.Text.Json.JsonSerializer.Serialize(result);
    }

    public bool TryGetResult(out T result)
    {
        result = _result!;
        return _result != null;
    }
}

public class MockChatClient : IChatClient
{
    private readonly Random _random = new();
    
    public ChatClientMetadata Metadata => new("MockChatClient", null, null);

    public void Dispose()
    {
        // No resources to dispose
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetResponseAsync (non-generic) was called");
        
        // Simulate some processing time
        await Task.Delay(500, cancellationToken);
        
        // Extract the system prompt to understand the sage's personality
        var systemMessage = chatMessages.FirstOrDefault(m => m.Role == ChatRole.System)?.Text ?? "";
        var userMessage = chatMessages.FirstOrDefault(m => m.Role == ChatRole.User)?.Text ?? "";
        
        // Mock decision making based on personality
        var (vote, reason) = GenerateMockResponse(systemMessage, userMessage);
        var sageResponse = new SageResponse(vote, reason);
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(sageResponse);
        
        // Create a ChatResponse with proper finish reason
        var chatMessage = new ChatMessage(ChatRole.Assistant, jsonResponse);
        var chatResponse = new ChatResponse(chatMessage)
        {
            FinishReason = ChatFinishReason.Stop
        };
        
        return chatResponse;
    }

    // This is the main method used by MagiService
    public async Task<TResult> GetResponseAsync<TResult>(IEnumerable<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        // Simulate some processing time
        await Task.Delay(500, cancellationToken);
        
        // Extract the system prompt to understand the sage's personality
        var systemMessage = chatMessages.FirstOrDefault(m => m.Role == ChatRole.System)?.Text ?? "";
        var userMessage = chatMessages.FirstOrDefault(m => m.Role == ChatRole.User)?.Text ?? "";
        
        // Mock decision making based on personality
        var (vote, reason) = GenerateMockResponse(systemMessage, userMessage);
        
        // Create the actual SageResponse
        var sageResponse = new SageResponse(vote, reason);
        
        // Create a mock AI response wrapper that matches the expected interface
        var mockResponse = new MockAIResponse<SageResponse>(sageResponse);
        
        // The interface expects us to return TResult, but TResult should be the response wrapper type
        // that contains the SageResponse, not SageResponse itself
        return (TResult)(object)mockResponse;
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Streaming not implemented for mock client");
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return serviceType.IsInstanceOfType(this) ? this : null;
    }

    private (VoteEnum vote, string reason) GenerateMockResponse(string personality, string userMessage)
    {
        // Simple mock logic based on personality keywords
        var isLogical = personality.Contains("論理") || personality.Contains("客観");
        var isConservative = personality.Contains("慎重") || personality.Contains("保守");
        var isEmotional = personality.Contains("感情") || personality.Contains("直感");
        
        // Random decision with personality bias
        var randomValue = _random.NextDouble();
        VoteEnum vote;
        string reason;
        
        if (isLogical)
        {
            vote = randomValue > 0.3 ? VoteEnum.Yes : VoteEnum.No;
            reason = vote == VoteEnum.Yes 
                ? "論理的に分析した結果、メリットがデメリットを上回ると判断します。"
                : "客観的なデータに基づくと、リスクが高すぎると考えられます。";
        }
        else if (isConservative)
        {
            vote = randomValue > 0.6 ? VoteEnum.Yes : VoteEnum.No;
            reason = vote == VoteEnum.Yes 
                ? "慎重に検討した結果、リスクを管理可能な範囲で進められると判断します。"
                : "保守的な観点から、現状維持が安全であると考えます。";
        }
        else if (isEmotional)
        {
            vote = randomValue > 0.4 ? VoteEnum.Yes : VoteEnum.No;
            reason = vote == VoteEnum.Yes 
                ? "直感的に良い方向に向かうと感じます。人々の感情的な反応も好意的でしょう。"
                : "感情的な観点から、この決断は人々に不安を与える可能性があります。";
        }
        else
        {
            vote = randomValue > 0.5 ? VoteEnum.Yes : VoteEnum.No;
            reason = vote == VoteEnum.Yes ? "賛成の理由があります。" : "反対の理由があります。";
        }
        
        return (vote, reason);
    }
}