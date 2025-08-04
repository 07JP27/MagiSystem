using System.Net;
using Microsoft.Extensions.AI;

namespace MagiSystem.Core;

public class MagiService
{
    private readonly List<Sage> _sages;

    public MagiService(IChatClient aiChatClient, List<Sage>? sages = null)
    {
        _sages = sages ?? new List<Sage>()
        {
            new Sage("論理・客観的な分析に偏る判断", aiChatClient),
            new Sage("慎重で保守的、リスク回避型", aiChatClient),
            new Sage("感情的・直感的な判断や妥協を提示", aiChatClient)
        };
    }


    public async Task<MagiResponse> MajorityVoteAsync(VoteOption option)
    {
        List<Task<SageResponse>> voteTasks = new();

        foreach (var sage in _sages)
        {
            voteTasks.Add(sage.VoteAsync(option));
        }

        var results = await Task.WhenAll(voteTasks);

        var response = new MagiResponse(
            FinalDecision: DetermineFinalDecision(results),
            CountOfYes: results.Count(r => r.VoteResult == VoteEnum.Yes),
            CountOfNo: results.Count(r => r.VoteResult == VoteEnum.No),
            YesReasons: results.Where(r => r.VoteResult == VoteEnum.Yes).Select(r => r.Reason).ToList(),
            NoReasons: results.Where(r => r.VoteResult == VoteEnum.No).Select(r => r.Reason).ToList()
        );

        return response;
    }

    private FinalDecisionEnum DetermineFinalDecision(SageResponse[] results)
    {
        int yesCount = results.Count(r => r.VoteResult == VoteEnum.Yes);
        int noCount = results.Count(r => r.VoteResult == VoteEnum.No);

        if (yesCount > noCount)
            return FinalDecisionEnum.Yes;
        else if (noCount > yesCount)
            return FinalDecisionEnum.No;
        else
            return FinalDecisionEnum.Tie;
    }
}
