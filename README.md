# Ruina Bugfix MODs

「Library of Ruina」のMODです。他のMODを使用したことにより発生した、ゲーム本体の動作バグを修正することを目的としています。
修正するバグ内容ごとにMODを分割しています。

## 具体的な修正内容

### ModAddedGiftsRemover (ModAddedGiftsRemover.dll)

- 他のMODで戦闘表象を追加したことにより、司書の設定画面がおかしくなる不具合を修正します。
  - 不具合の例: 画面が切り替わらずに重なったようなレイアウトとなる、戦闘表象の説明テキストが「□□□」のようになる、戦闘表象が装備できない等。

### InvalidPassivesRemover (InvalidPassivesRemover.dll)

- 他のMODで装備可能なパッシブ (ページ) を追加し、そのMODを削除した事により、コアページ編集ができなくなる不具合を修正します。

## 免責事項

**このMODの使用により発生した問題等は、いかなる理由があっても一切責任を負いません。**

そのため、万が一に備えてセーブデータのバックアップを行ってからこのMODを使用するといった自己防衛策を推奨します。

Library of Ruinaのセーブデータは以下の場所に保存されています。

`%UserProfile%\AppData\LocalLow\Project Moon\LibraryOfRuina`

## ダウンロード方法

1. [「Releases」ページ](https://github.com/TanaUmbreon/RuinaBugfixMods/releases) を開きます。
2. 最新バージョンの「Assets」から「RuinaBugfixMods-bin-*x*.*x*.*x*.zip」をクリックしてダウンロードしてください。

## リリースノート

### Version 1.1.0 (2020/12/16)

- `InvalidPassivesRemover` (他のMODで追加して削除したパッシブによりコアページ編集ができなくなるバグについての修正MOD) を作成。

### Version 1.0.0 (2020/12/13)

- `ModAddedGiftsRemover` (他のMODにより追加された戦闘表象を削除するバグ修正MOD) を作成。
