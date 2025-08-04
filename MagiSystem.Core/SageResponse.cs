using Microsoft.Extensions.AI;

namespace MagiSystem.Core;

public record SageResponse(VoteEnum VoteResult, string Reason);
