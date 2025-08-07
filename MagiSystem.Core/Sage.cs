using Microsoft.Extensions.AI;

namespace MagiSystem.Core;

public class Sage(string personality, IChatClient aiChatClient)
{
    private static readonly ChatOptions _voteChatOption = new()
    {
        ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(InternalSageResponse))),
    };

    private record InternalSageResponse(VoteEnum VoteResult, string Reason);

    private readonly string _systemPrompt = $"""
        あなたは合議制における投票権を持つ一人の賢者です。与えられた議題に対して投票を行うことが求められます。
        あなたのパーソナリティは「{personality}」です。必ずこのパーソナリティに従って行動してください。
        """;

    public async Task<SageResponse> VoteAsync(VoteOption option)
    {
        var messages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, _systemPrompt),
            new ChatMessage(ChatRole.User, GetUserMessage(option))
        };

        var result = await aiChatClient.GetResponseAsync<InternalSageResponse>(messages, _voteChatOption);
        if (result.FinishReason != ChatFinishReason.Stop)
        {
            throw new InvalidOperationException($"AI chat completion failed with finish reason: {result.FinishReason}");
        }

        if (!result.TryGetResult(out var sageVoteResponse))
        {
            // JSONパースに失敗した場合のフォールバック処理
            throw new InvalidOperationException($"Failed to parse AI response to GenerateQueryResponse. Raw response: {result.Text}");
        }

        return new(personality, sageVoteResponse.VoteResult, sageVoteResponse.Reason);
    }


    private string GetUserMessage(VoteOption voteOption)
    {
        return $"""
        ## Topic
        次の議題について投票してください。
        --------
        {voteOption.Topic}
        --------
        Yesの判断基準: {voteOption.YesCriteria}
        Noの判断基準: {voteOption.NoCriteria}
        """;
    }
}
