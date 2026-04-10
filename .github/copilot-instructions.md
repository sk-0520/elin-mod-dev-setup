# Copilot に対する指示

* レビューや出力内容のすべては日本語で記載すること
* 試行中のセッション内容やエラー、ログなどの出力も日本語で表示する
* 提案コードには可能な限り日本語のコメントをつけること
* 表示するフィードバックは簡潔かつわかりやすい日本語で書くこと
* typo などはコード・ファイル名・フォルダ名に関わらず指摘せよ(質問プロンプトに対しては指摘不要)
* 不明な点があれば、質問を投げかけること
* 可能な限り関連ソースを精査せよ

## このリポジトリについて

* 本リポジトリは、ゲーム Elin の Mod 作成を支援するためのテンプレートとなる
  * Elin: <https://store.steampowered.com/app/2135150/Elin>
  * Mod
    * <https://ylvapedia.wiki/wiki/Elin:Mod>
    * <https://elin-modding-resources.github.io/Elin.Docs>

## ファイル構成

* dev-items
  * テンプレート側提供の各種定義・実行処理を格納
  * ビルド時に裏で動くような Mod 開発者にとっては関心が薄いけれどもテンプレートとしては必要な処理等
* docs
  * 本リポジトリの説明文書
* Elin.Plugin.Generator
  * Mod 作成において定義ファイルなどから Mod 用の各種テンプレート提供機能を生成するソースジェネレーター
* Elin.Plugin.Generator.Test
  * Elin.Plugin.Generator のテストコード
* Elin.Plugin.Main
  * Mod 開発者が実装する Mod の本体
  * 最低限のサンプルと、テンプレート提供機能が格納されている
* Elin.Plugin.Main.Test
  * Elin.Plugin.Main のテストコード
  * テストコード自体は Mod 開発者が必要に応じて実装する
