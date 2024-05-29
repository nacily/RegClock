# RegClock
趣味アプリ。いわゆる別の世界の時計です  
検索回避のため、固有名詞はあえて書いていません。  
  
## 機能仕様
1. 現実時間
 - 現実世界の時間を表示しています。  
　 DateTime.Nowで取得していますが、動作の都合上-1秒ほど遅いです。  
　 JST Clockのサイトをにらめっこするとわかります。あまり重要ではないので修正予定はありません。  
2. 内部時間
 - 内部時間を表示しています。  
　 現実時間の28.8倍の速さで進む世界線なので、秒の表示はありません。時と分のみです。  
　 また、本家のなんちゃら時計と比べると上記現実時間が遅れている関係か、こっちも遅れています。  
　 年月日も表示しようか悩みましたが、暦（閏年の扱いとか）が面倒なのでやめました。  
　 なんで14:34加算してるの？については私もよくわかってません。(なんでだ)  
　 でも本家の時計と比べるとそれ加算しないとズレるんです...深く考えてはいけない。  
  
3. 潮時間
 - 某マップの潮の現在の状態と、残り時間を現実時間で表示しています。  
　 これも現実時間の時計が若干遅れているので、誤差1秒はあるかもしれません。  
　 なお切り替わりは「完全に満ちた時間」と「完全に干いた時間」ですので、  
　 「満ちはじめ」「干きはじめ」は前のフェーズに含まれています。ご注意ください。  
4. レイド時間
 - レイドのお時間です。たぶんあってますが、1週間張り付いて確認したわけではないので  
　 もしかしたら間違いあるかもしれません。まぁ暗記すればいいので別に困りませんよね‥。  
5. 消耗品タイマー
 - よく使うであろう、某消耗品関係のタイマーです。  
　 中には3d/7dというアイテムもありますが、外部で開始時間を保持するのは面倒なのでしません。  
　 基本的にアプリ起動中に短時間で終わるもの、だけ入れてあります。  
  
## 使用方法
・各自ビルドしてください  
・.NET 8.0以降が必要かと思います。各自ご用意ください。  
　なお環境変数Pathへの"C:\Program Files\dotnet"の追加は、  
　(x86)版のSDKよりも上に配置しないと読み込んでくれません。ご注意を。  
  
## 免責事項
　不具合、たぶんあります。見つけたら直しますが報告は不要です。  
　自分用に作ったので、周囲が使うことを想定していません。  
　トラブルやBANなどの諸問題に関しては一切の責任を持ちません。  
  
2024.5.29 梨莉  
  
