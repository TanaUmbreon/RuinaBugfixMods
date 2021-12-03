# Ruina Bugfix MODs

「Library Of Ruina」のMODです。他のMODを利用することで発生することがあるバグを修正します。

以下の通り、修正機能ごとにMODを分割しています。

**※注意**: このMODは公式の一般招待状MODではなく、**BongBong Enterprises版BaseModsを導入している環境で動作します**。

## 修正機能の詳細

### InvalidGiftsRemover (InvalidGiftsRemover.dll)

セーブデータに保存されている、**無効な戦闘表象を削除します**。

この「無効な戦闘表象」は以下の状況で確実に発生し、ゲームを起動して「続きから」を選択しても先に進めなくなる不具合が発生します。

- 戦闘表象を追加しているMODを導入した時。
- かつ、その戦闘表象を入手した状態でセーブデータが保存された時。
- かつ、そのMODを無効化または削除した状態でゲームを起動した時。

### InvalidPassivesRemover (InvalidPassivesRemover.dll)

セーブデータに保存されている、**コアページに帰属中の無効なパッシブを削除します**。

この「無効なパッシブ」は以下の状況で発生する可能性があり、ゲームを起動して「続きから」を選択しても先に進めなくなる、コアページ編集ができなくなるといった不具合が発生します。

- 帰属可能なパッシブを追加しているMODを導入した時。
- かつ、そのパッシブを他のコアページに帰属した状態でセーブデータが保存された時。
- かつ、そのMODを無効化または削除した状態でゲームを起動した時。

## 免責事項

**このMODの使用により発生した問題等は、いかなる理由があっても一切責任を負いません。**

## 推奨事項

万が一に備えてセーブデータをバックアップしてからこのMODを使用するといった自己防衛策を推奨します。

Library Of Ruinaのセーブデータは以下の場所に保存されています。

`%UserProfile%\AppData\LocalLow\Project Moon\LibraryOfRuina`

また、必要な時だけこのMODを使用し、使用後はゲーム起動時に無効化またはBaseModsから削除することを推奨します。

## ダウンロード方法

1. [「Releases」ページ](https://github.com/TanaUmbreon/RuinaBugfixMods/releases) を開きます。
2. 最新バージョンの「Assets」から「RuinaBugfixMods-bin-*x*.*x*.*x*.zip」をクリックしてダウンロードしてください。

## リリースノート

### Version 2.0.0 (2021/12/3)

- 本体バージョン1.1.0.6a (BongBong Enterprises版BaseMods環境) に対応。
- `ModAddedGiftsRemover` のMOD名を `InvalidGiftsRemover` に変更。
  - 同時に、修正機能を「特定のID値以降の戦闘表象を全て削除する」から「無効な戦闘表象を全て削除する」に変更。

### Version 1.1.0 (2020/12/16)

- `InvalidPassivesRemover` (他のMODで追加して削除したパッシブによりコアページ編集ができなくなるバグについての修正MOD) を作成。

### Version 1.0.0 (2020/12/13)

- `ModAddedGiftsRemover` (他のMODにより追加された戦闘表象を削除するバグ修正MOD) を作成。
