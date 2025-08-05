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

### インストール

1. **リポジトリのクローン**
   ```bash
   git clone https://github.com/07JP27/MagiSystem.git
   cd MagiSystem
   ```

2. **Azure OpenAI設定の構成**
   
   `MagiSystem.Web/appsettings.json` を編集するか、環境変数を設定します：
   ```json
   {
     "AzureOpenAI": {
       "Endpoint": "https://your-resource.openai.azure.com/",
       "ApiKey": "your-api-key",
       "DeploymentName": "gpt-35-turbo"
     }
   }
   ```

   または環境変数を使用：
   ```bash
   export AzureOpenAI__Endpoint="https://your-resource.openai.azure.com/"
   export AzureOpenAI__ApiKey="your-api-key"
   export AzureOpenAI__DeploymentName="gpt-35-turbo"
   ```

3. **ビルドと実行**
   ```bash
   dotnet build
   cd MagiSystem.Web
   dotnet run
   ```

4. **アプリケーションへのアクセス**
   
   ブラウザで `https://localhost:5001`（またはターミナルに表示されるURL）にアクセスします。

### 使用方法

1. **議題の入力**: 決定が必要な議題を記述します
2. **投票基準の設定**: 「Yes」と「No」の判断基準を定義します
3. **投票の実行**: 3つのMAGIがあなたの議題を分析し投票を行います
4. **結果の確認**: 各賢者の詳細な理由と共に多数決による決定を確認します

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