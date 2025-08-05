using Microsoft.Extensions.AI;
using MagiSystem.Core;
using Azure;
using Azure.AI.OpenAI;


    
var chatClient = new AzureOpenAIClient(
    new Uri("https://your-resource.openai.azure.com/"),
    new AzureKeyCredential("your-api-key") // DefaultAzureCredential can also be used
    ).GetChatClient("gpt-4o")
    .AsIChatClient();

var magiService = new MagiService(chatClient);

System.Console.WriteLine("=== Magi System Console ===");
System.Console.WriteLine();

while (true)
{
    System.Console.WriteLine("投票する議題を入力してください (終了するには 'exit' を入力):");
    var topic = System.Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(topic) || topic.ToLower() == "exit")
        break;
    
    System.Console.WriteLine();
    System.Console.WriteLine("Yes判定の基準を入力してください:");
    var yesCriteria = System.Console.ReadLine();
    
    System.Console.WriteLine();
    System.Console.WriteLine("No判定の基準を入力してください:");
    var noCriteria = System.Console.ReadLine();
    
    var voteOption = new VoteOption(topic, yesCriteria ?? "", noCriteria ?? "");
    
    System.Console.WriteLine();
    System.Console.WriteLine("Magi システムで投票を実行中...");
    System.Console.WriteLine();
    
    try
    {
        var result = await magiService.MajorityVoteAsync(voteOption);
        
        DisplayResult(result);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"エラーが発生しました: {ex.Message}");
    }
    
    System.Console.WriteLine();
    System.Console.WriteLine("--- 次の投票 ---");
    System.Console.WriteLine();
}

System.Console.WriteLine("Magi system done.");
    
    
static void DisplayResult(MagiResponse response)
{
    System.Console.WriteLine("=== MAGI投票結果 ===");
    System.Console.WriteLine($"最終決定: {GetDecisionText(response.FinalDecision)}");
    System.Console.WriteLine($"Yes票: {response.CountOfYes}票");
    System.Console.WriteLine($"No票: {response.CountOfNo}票");
    System.Console.WriteLine();
    
    if (response.YesReasons.Any())
    {
        System.Console.WriteLine("【Yes理由】");
        foreach (var reason in response.YesReasons)
        {
            System.Console.WriteLine($"• {reason}");
        }
        System.Console.WriteLine();
    }
    
    if (response.NoReasons.Any())
    {
        System.Console.WriteLine("【No理由】");
        foreach (var reason in response.NoReasons)
        {
            System.Console.WriteLine($"• {reason}");
        }
        System.Console.WriteLine();
    }
}

static string GetDecisionText(FinalDecisionEnum decision)
{
    return decision switch
    {
        FinalDecisionEnum.Yes => "承認",
        FinalDecisionEnum.No => "否決",
        FinalDecisionEnum.Tie => "引き分け",
        _ => "不明"
    };
}
