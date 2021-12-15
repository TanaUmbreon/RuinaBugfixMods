=================
Ruina Bugfix MODs
=================

ゲーム「Library Of Ruina」のMOD第1弾です。
他のMODを利用することで発生することがあるバグを修正します。

以下の通り、修正機能ごとにMODを分割しています。

- Invalid Gifts Remover (InvalidGiftsRemover.dll)
- Invalid Passives Remover (InvalidPassivesRemover.dll)

-----------------------
1. 注意事項・免責事項等
-----------------------

- このMODは公式の一般招待状MODではなく、BongBong Enterprises版BaseModsを導入している環境で動作します。
- このMODの使用により発生した問題等は、いかなる理由があっても一切責任を負いません。
- 万が一に備えてセーブデータをバックアップしてからこのMODを使用するといった自己防衛策を推奨します。

----------------
2. 詳細なMOD解説
----------------

下記Wikiページを参照してください。

https://github.com/TanaUmbreon/RuinaBugfixMods/wiki

-----------
3. 使用方法
-----------

「2. 詳細なMOD解説」のリンク先をきちんと読んだ上で使用してください。

修正機能ごとにMODをフォルダ分けしています。
このREADMEファイルと同じフォルダに置かれているフォルダをBaseMods直下に配置することで動作します。

[RuinaBugfixMods-bin-x.x.x]
├ [InvalidGiftsRemover] ←このフォルダをBaseMods直下に配置する
│  └[InvalidGiftsRemover.dll]
├ [InvalidPassivesRemover] ←このフォルダをBaseMods直下に配置する
│  └[InvalidPassivesRemover.dll]
└ [README(ja).txt] ←今開いているファイル


ゲームを起動して「続きから」を選択すると修正機能が動作します。
「続きから」の画面から先に進めるようになっていれば正常に動作しています。

画面が先に進んだ後は、ゲーム終了時に自動セーブが行われる操作(司書のデッキやパッシブ、戦闘表象の装備を変更する等)を必ず行ってください。
これを行わない場合は、修正がセーブデータに反映されず、ゲーム起動前の状態に戻ってしまいます。

このMODが実行されると各MODフォルダの直下にログファイルが作成されます。
どのデータが削除されたか確認することができます。
