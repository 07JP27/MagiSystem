namespace MagiSystem.Core;

public record MagiResponse(FinalDecisionEnum FinalDecision, int CountOfYes, int CountOfNo, List<SageResponse> YesResponses, List<SageResponse> NoResponses);