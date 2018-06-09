# Findings Editor（内視鏡・超音波検査所見入力システム）

 【動作環境】Windows XP/7/8/8.1/10 
 【取り扱い種別】フリーソフト  
 【開発】（株）[メイドインクリニック](http://www.madeinclinic.jp)  
 【開発環境】Microsoft Visual Studio 2017 (Visual C# 2017)

## 概要

内視鏡や超音波検査の所見、貴院ではどうされてますか？  
紙に書かれていますか？  
電子カルテに直接記入？  
内視鏡メーカーのシステムで入力？  
紙ではかさばりますし、どうしても閲覧性に欠けてしまいます。  
他のメーカーの内視鏡の所見は入力できますか？  
システムが古くなった時、システムの入れ替えは簡単にできますか？  
（ハードディスクの故障率は３年目以降に急激に上昇するといわれています。）   
そもそも、超音波検査の所見入力システムって、あまり選べないですよね…。  
このFindings Editorはそれらを__解決する__システムです。  
データベースにはオープンソースソフトウェアであるPostgreSQLを採用しており、サーバー等のハードウェアの入れ替えも容易なほか、大病院などの大規模環境にも対応することができます。   
運用には、普段利用しているWindows PCが１台あれば問題ありません  
（セキュリティ上の都合からWindows 8.1pro推奨）が、数十台以上の環境にも容易に対応が可能です。  
現在開発中のFindings Siteと組み合わせることで、iPadやAndroidタブレットでの閲覧も可能になります。  
OSが英語のPCでは自動的に英語表示に切り替わります。  
詳細なインストール、運用を解説する「Findings Editor準備・運用マニュアル」や、設置するだけで使用することができるリファレンス機を近日発売予定です。  
詳しくは当社ホームページをご確認ください。  
また、大規模環境への導入にはコンサルティング業務も行う予定です。  
　  
日本消化器内視鏡学会の[JED project](http://www.jges.net/jedproject)にも対応を予定しています。  

## 動作環境

Windows XP、7、8、8.1での動作を確認しています。  
ただし、Windows XPはサポート期限が終了しているため、推奨しません。

## 動作に必要なソフトウェア

.NET Framework 4.0が必要です。  
.NET Framework 4.0はWindows updateでインストールが可能です。  
　  
インターネットに接続していないPCでFindings Editor（以下、本ソフトウェア）が動作しないようであれば、  
[http://www.microsoft.com/ja-jp/download/details.aspx?id=24872](http://www.microsoft.com/ja-jp/download/details.aspx?id=24872)  
にアクセスし、dotNetFx40_Client_x86_x64.exeをダウンロードしてインストールしてください。  
　  
また、「Windows Imaging Component (WIC) をインストールする必要があります。」  
と表示されることがあります。その際は、  
【32bit環境の場合】[http://www.microsoft.com/ja-jp/download/details.aspx?id=32](http://www.microsoft.com/ja-jp/download/details.aspx?id=32)  
【64bit環境の場合】[http://www.microsoft.com/ja-jp/download/details.aspx?id=1385](http://www.microsoft.com/ja-jp/download/details.aspx?id=1385)  
にアクセスしてWindows Imaging Componentをダウンロードし、インストールしてください。  
　  
なお、これらを電子カルテ端末などにインストールされた場合、既存のソフトの動作に支障をきたす場合がありますので、電子カルテ端末への本ソフトウェアのインストールは推奨しません。  
インストールが可能かどうかは、各カルテメーカーにお問い合わせください。  
なお、本ソフトウェアを使用して発生した損害等について、当社は一切の責任を負いかねますのでご了承ください。  

##　インストール方法

1.[サーバー証明書の作成](./SSLCRT-ja.md)  
2.[データベースの導入（PostgreSQL）](./POSTGRESQL-ja.md)  
3.[Findings Editorの導入](./FEINSTALL-ja.md)  

## 運用方法

4.[Findings Editorの初期設定](./FEINISET-ja.md)  
5.[新規患者・検査の登録](./FERUNNING01-ja.md)  
6.[所見の入力と印刷](./FERUNNING02-ja.md)  
7.[便利な機能](./FERUNNING03-ja.md)  

## その他

8.[その他](./ETCETERA-ja.md)  

## 起動方法

FE.exeをダブルクリックするとFindings Editorが起動します。  
ショートカットをデスクトップに置くなどしてご利用ください。  
なお、Windows8以降ではWindows SmartScreenによって起動できない場合があります。  
この場合は、ファイルのプロパティから「ブロックの解除」を行ってください。  

## アンインストール方法

フォルダごと削除してください。  
その他の操作は必要ありません。  

## 使い方と事前準備  

データベース初期設定ソフトとして、「FE_setup」（フリーソフト）を別に公開しています。  
PostgreSQLの動作している環境で「FE_setup」を実行することにより、本ソフトウェアをご利用いただく環境をすぐに構築することができます。  
また、所見を印刷する際のフッター（医療機関名）は同梱の「result.html」を適宜編集してください。   
　  
### FE.exe  

FE.exeを起動し、初期設定でDBサーバ（PostgreSQL）のIPアドレスまたはホスト名、ポート番号、DBのID、パスワード、シェーマ保存フォルダを指定します。  
DBサーバIPアドレス：localhost（データベースをインストールしたマシンで利用する場合）  
DBサーバのポート：5432  
DBのID：db_user  
DBのパスワード：test（FE_setupで変更されることを推奨いたします）  
シェーマ保存フォルダ：任意のフォルダ（サーバーの共有フォルダやNASをネットワークドライブとして設定し、そこを設定することをお勧めいたします。）  
　  
初めに、IDを「test」、パスワードを「test」としてログインし、「オプション」の「管理」→「施行者」から管理用IDやパスワードを設定してください。  
設定後、ID「test」を削除してください。  
なお、カテゴリー「Viewer」は所見入力ができないユーザーとなります。  
事務の方がご利用になる場合にご使用ください。  
また、別ソフトのFindingsSiteでもViewerカテゴリーのユーザーを利用します。  
　  
次に、ログイン後のオプションの管理メニューから検査機器や検査場所等の必要な設定を行った後、新規患者を登録し、登録が終わったら新規検査を登録してください。  
内視鏡や超音波の検査を施行したら、施行者が所見を入力し、1次診断医による所見、2次診断医による所見等を入力することができます。  
また、これらの所見には同梱の患者画像管理ソフト（画像登録：GraphicRenamer, 画像閲覧：PtGraViewer）で患者に紐付く画像を登録、閲覧することができます。  
PtGraViewerは検査リストの画像ボタンから起動可能です。  
　  
便利な機能として、検査種別（CF、GFなど）ごとに自動入力される既定の所見を設定したり、よく使うワードを登録してワンクリックで入力することができます。  
「オプション」のマイワードや、「管理」の「クイックワード」、「既定の所見」等で設定してください。  
　  
### 患者情報取得用のプラグインについて  

電子カルテ等から患者情報を取得するためのプラグインに対応しました。  
現在、正式に対応できているのはORCAのみです。  
他の電子カルテシステムについては、お問い合わせください。  

## 制限事項

初期状態で起動した場合は、データベースにデータが存在しないため、各動作でエラーメッセージが表示されることがありますが、データを入力することで表示されなくなります。  
また、本ソフトウェアで入力した患者情報や所見、設定した既定の所見やマイワード等はすべてPostgreSQLに暗号化されずに保存されます。  
このため、データベースシステムの入れ替え等は簡単に行うことができますが、個人情報の保護等については、PostgreSQLがインストールされている環境を丸ごと暗号化するなどの対策（WindowsのBitLockerなど）を適宜行ってください。  

## 連絡先  
ご不明な点、ご要望等ございましたら、こちらまでご連絡下さい。  
URL: [http://www.madeinclinic.jp](http://www.madeinclinic.jp)
E-mail: [info@madeinclinic.jp](info@madeinclinic.jp)

## 著作権および免責事項

本ソフトウェアはフリーソフトです。（GPL v3 ライセンス）  
自由に使用していただいて構いませんが、  著作権は株式会社メイドインクリニックが保有しています。  
なお、このソフトウェアを使用したことによって生じたすべての障害・損害・不具合等に関し、当社は一切の責任を負いません。