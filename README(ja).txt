=================
Ruina Bugfix MODs
=================

「Library of Ruina」のMODです。
他のMODを使用したことにより発生した、ゲーム本体の動作バグを修正することを目的としています。


-----------
1. 免責事項
-----------

このMODの使用により発生した問題等は、いかなる理由があっても一切責任を負いません。


-----------
2. 推奨事項
-----------

万が一に備えてセーブデータのバックアップを行ってからこのMODを使用するといった自己防衛策を推奨します。
Library of Ruinaのセーブデータは以下の場所に保存されています。
"%UserProfile%\AppData\LocalLow\Project Moon\LibraryOfRuina"

また、必要な時だけこのMODを使用し、使用後はBaseModsから削除することを推奨します。


-----------
3. 使用方法
-----------

修正するバグ内容ごとにMODをフォルダ分けしています。
このREADMEファイルと同じフォルダに置かれているフォルダをBaseMods直下に配置することで動作します。

[RuinaBugfixMods-bin-x.x.x]
├ [Z_ModAddedGiftsRemover] ←このフォルダをBaseMods直下に配置する
│  └[ModAddedGiftsRemover.dll]
└ [README(ja).txt] ←今開いているファイル

また、以下の「4. 各MODの詳細と注意事項」をきちんと読んでください。
MODによっては使用方法が特殊であり、方法を誤るとバグ修正が適用されないといった問題が発生します。


------------------------
4. 各MODの詳細と注意事項
------------------------

<<Z_ModAddedGiftsRemover (ModAddedGiftsRemover.dll)>>

セーブデータのロード時に、他のMODで追加された戦闘表象をすべて削除します。
他のMODで戦闘表象を追加したことにより、司書の設定画面で不具合が出る場合にのみ使用してください。

具体的に削除対象となる戦闘表象は、ID値が0～47以外のものとなります。
このID値の範囲は、2020年12月5日(本体バージョン1.0.3.4)時点でオリジナルの方に実装されている戦闘表象となります。

セーブデータに他のMODで追加された戦闘表象が含まれている状態だと、
その戦闘表象が含まれるMODをBaseModsから削除してしまうとタイトル画面で「続きから」を選択できません。
そのため、BaseModsに戦闘表象が含まれるMODと当MODを同時に入れた状態でゲームを起動してください。

その時、読み込まれるMODの順番が当MODが最後となるようにします。
MODが読み込まれる順番は、BaseMods直下の各MODフォルダを昇順で並び替えた時の並び順となります。
配布時点でフォルダ名に"Z_"をつけているのでそのままでも最後の方に読み込まれますが、
必要に応じてフォルダ名を変更してください。

ゲーム起動後は「続きから」を選択してセーブデータを読み込んでください。
その後、司書の設定画面を表示させて、他MODで追加された戦闘表象が削除されて問題が解消されていることを確認してください。
確認が完了後は、ゲーム終了時に自動セーブが行われる操作(司書のデッキやパッシブ、戦闘表象の装備を変更する等)を必ず行ってください。
これを行わない場合は、この修正MODがセーブデータに反映されず、ゲーム起動前の状態に戻ってしまいます。

このMODが実行されるとログファイル"ModAddedGiftsRemoverLog.txt"がBaseMods直下に作成されます。
どの戦闘表象が削除されたか確認することができます。