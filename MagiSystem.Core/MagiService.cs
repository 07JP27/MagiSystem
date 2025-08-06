using System.Collections.ObjectModel;
using Microsoft.Extensions.AI;

namespace MagiSystem.Core;

public class MagiService
{
    private readonly ReadOnlyCollection<Sage> _sages;

    public MagiService(IChatClient aiChatClient)
    {
        _sages = new ReadOnlyCollection<Sage>(
        [
            new Sage("論理・客観的な分析に偏る判断", aiChatClient),
            new Sage("慎重で保守的、リスク回避型", aiChatClient),
            new Sage("感情的・直感的な判断や妥協を提示", aiChatClient)
        ]);
    }

    public MagiService(ReadOnlyCollection<Sage> sages)
    {
        _sages = sages;
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
            YesResponses: results.Where(r => r.VoteResult == VoteEnum.Yes).ToList(),
            NoResponses: results.Where(r => r.VoteResult == VoteEnum.No).ToList()
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
