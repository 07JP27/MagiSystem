namespace MagiSystem.Core;

public record MagiResponse(FinalDecisionEnum FinalDecision, int CountOfYes, int CountOfNo, List<VoteReason> YesReasons, List<VoteReason> NoReasons);