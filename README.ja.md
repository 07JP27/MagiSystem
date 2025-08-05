# MagiSystem

新世紀エヴァンゲリオンのMAGIシステムからインスピレーションを得た意思決定支援システムです。異なる人格を持つ3つのAI賢者が多数決による協調により、バランスの取れた意思決定を提供します。

[English version here](README.md)

## 概要

MagiSystemは、それぞれ異なる思考パターンを持つ3つのAIエージェント（賢者）の集合知を活用し、民主的な投票を通じて意思決定を支援します。各AIは独自の視点から与えられた議題を分析し投票を行い、多数決により最終決定が下されます。

### 3人の賢者

- **🧠 論理型**: 論理的・客観的な分析に基づいて判断を行い、データとエビデンスを重視します
- **🛡️ 慎重型**: 慎重で保守的、リスク回避を重視したアプローチで、安全性と安定性を最優先に考えます
- **❤️ 感情型**: 感情的・直感的な判断を提供し、人間らしい視点と妥協案を大切にします

## クイックスタート

### 前提条件

- .NET 8.0 SDK
- Azure OpenAI Service アカウントとAPI アクセス

### MagiServiceの利用方法

MagiSystem.Coreライブラリは、あなたのアプリケーションに3つの賢者による意思決定機能を統合するためのコアサービスを提供します。

#### 1. プロジェクトへの参照追加

```bash
# プロジェクトの参照を追加
dotnet add reference path/to/MagiSystem.Core

# または、NuGetパッケージとして利用する場合（将来的に公開される予定）
# dotnet add package MagiSystem.Core
```

#### 2. Azure OpenAI クライアントの設定

```csharp
using Microsoft.Extensions.AI;
using MagiSystem.Core;

// Azure OpenAI クライアントを設定
var chatClient = new AzureOpenAIClient(
    new Uri("https://your-resource.openai.azure.com/"),
    new AzureKeyCredential("your-api-key"))
    .AsChatClient("gpt-35-turbo");
```

#### 3. MagiServiceインスタンスの作成

```csharp
// デフォルトの3つの賢者でMagiServiceを作成
var magiService = new MagiService(chatClient);

// または、カスタム賢者を指定
var customSages = new List<Sage>
{
    new Sage("データ重視の論理的判断", chatClient),
    new Sage("リスク管理を重視する慎重な判断", chatClient),
    new Sage("ユーザー体験を重視する感情的判断", chatClient)
};
var magiService = new MagiService(chatClient, customSages);
```

#### 4. 投票の実行

```csharp
// 投票オプションを作成
var voteOption = new VoteOption(
    Topic: "新機能Aを次のリリースに含めるべきか？",
    YesCriteria: "ユーザーにとって価値があり、技術的に実現可能である",
    NoCriteria: "リスクが高く、開発リソースが不足している"
);

// 3つの賢者による投票を実行
MagiResponse response = await magiService.MajorityVoteAsync(voteOption);

// 結果の確認
Console.WriteLine($"最終決定: {response.FinalDecision}");
Console.WriteLine($"Yes票: {response.CountOfYes}, No票: {response.CountOfNo}");

// 各賢者の理由を表示
Console.WriteLine("Yes票の理由:");
foreach (var reason in response.YesReasons)
{
    Console.WriteLine($"- {reason}");
}

Console.WriteLine("No票の理由:");
foreach (var reason in response.NoReasons)  
{
    Console.WriteLine($"- {reason}");
}
```

### サンプルWebアプリケーション

実際の動作を確認したい場合は、付属のWebアプリケーションを実行できます：

```bash
git clone https://github.com/07JP27/MagiSystem.git
cd MagiSystem/MagiSystem.Web

# appsettings.jsonでAzure OpenAI設定を構成
dotnet run
```

ブラウザで `https://localhost:5001` にアクセスしてWebインターフェースを使用できます。

## カスタマイズ

### 新しい賢者人格の追加

`MagiSystem.Core/MagiService.cs` の `MagiService` コンストラクタを修正することで、賢者の人格をカスタマイズできます：

```csharp
_sages = sages ?? new List<Sage>()
{
    new Sage("カスタム人格の説明", aiChatClient),
    new Sage("別の人格タイプ", aiChatClient),
    new Sage("第三の人格バリエーション", aiChatClient)
};
```

### システムの拡張

- **カスタムAIプロバイダー**: 異なるAIサービスを使用するために独自の `IChatClient` を実装
- **追加の賢者**: 3人以上の賢者をサポートするようにシステムを変更
- **カスタム投票ロジック**: 異なる決定アルゴリズムのために `DetermineFinalDecision` メソッドをオーバーライド
- **UIカスタマイズ**: `MagiSystem.Web/Components` 内のBlazorコンポーネントを修正してインターフェースを変更

### 設定オプション

システムは `appsettings.json` を通じて様々な設定オプションをサポートします：

```json
{
  "AzureOpenAI": {
    "Endpoint": "Azure OpenAIエンドポイント",
    "ApiKey": "APIキー", 
    "DeploymentName": "モデルデプロイメント名"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## アーキテクチャ

### プロジェクト構造

- **MagiSystem.Core**: コアロジックとドメインモデル
  - `MagiService`: 投票プロセスを統括するメインサービス
  - `Sage`: 人格に基づく意思決定を行う個別のAIエージェント
  - モデル: `VoteOption`, `MagiResponse`, `SageResponse` など

- **MagiSystem.Web**: Blazor Server ウェブアプリケーション
  - ユーザーインターフェース用のRazorコンポーネント
  - コアサービスとの統合
  - リアルタイム投票結果表示

### 技術スタック

- **.NET 8**: フレームワークとランタイム
- **Blazor Server**: インタラクティブなWeb UIフレームワーク
- **Microsoft.Extensions.AI**: AI統合の抽象化
- **Azure OpenAI**: AI言語モデルプロバイダー
- **Bootstrap 5**: レスポンシブUI用CSSフレームワーク

## コントリビューション

1. リポジトリをフォーク
2. フィーチャーブランチを作成 (`git checkout -b feature/amazing-feature`)
3. 変更をコミット (`git commit -m 'Add some amazing feature'`)
4. ブランチにプッシュ (`git push origin feature/amazing-feature`)
5. プルリクエストを開く

## ライセンス

このプロジェクトはMITライセンスの下でライセンスされています - 詳細は [LICENSE](LICENSE) ファイルを参照してください。

## インスピレーション

このプロジェクトは、アニメシリーズ「新世紀エヴァンゲリオン」のMAGIシステムからインスピレーションを得ています。MAGIシステムでは、異なる人格的側面を持つ3台のスーパーコンピューターが協力して重要な決定を下します。私たちの実装ではスーパーコンピューターの代わりにAI言語モデルを使用していますが、多様な視点が集合的意思決定に貢献するという核となるコンセプトは同じです。