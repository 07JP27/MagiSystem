namespace MagiSystem.Core;

public record MagiResponse(FinalDecisionEnum FinalDecision, int CountOfYes, int CountOfNo, List<string> YesReasons, List<string> NoReasons);