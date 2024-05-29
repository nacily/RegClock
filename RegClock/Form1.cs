namespace RegClock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 共通変数 内部時間 (年月日に関しては暦仕様要検討につき未実装)
        static int regDay = 0;
        static int regHour = 0;
        static int regMinute = 0;

        const string txt_CurrentSeaLv_Empty = "ラウェハラ岬 潮時刻 : 現在 干潮";
        const string txt_CurrentSeaLv_Full = "ラウェハラ岬 潮時刻 : 現在 満潮";

        // 現在参照中のフェーズ (初期値 フェーズ0 どのフェーズでも初回に必ず処理実行)
        int currentPhase = 0;

        // 現実時間と内部時間のタイマー変数 同時進行するため共通化
        System.Timers.Timer tm1 = new(1000);
        System.Timers.Timer tm2 = new(1000);

        // 次の潮位変動までの残り時間 (現実時間基準)
        DateTime seaLvRemainTime = new DateTime();
        string txtMsg = "";

        // 次回レイド･防衛戦開始時刻
        DateTime nextRaidDate = new DateTime();
        // レイド･防衛戦 開催中メッセージ
        string raidOnGoingMsg = "";

        // 消耗品タイマー用
        System.Timers.Timer tm3 = new(1000);
        // 消耗品タイマー用DateTime変数
        DateTime consumeTimer = new DateTime();

        // 共通変数(クラス変数)はここに記載する… //

        /// <summary>
        /// フォームロード時に呼び出されるメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            getCurrentRegDateTime();

            timer1.Start();
            timer2.Start();

            groupBox3.Text = String.Empty;
        }

        /// <summary>
        /// 現在の内部時間を取得するためのメソッド
        /// フォームロード時と、timer2で1000ミリ秒毎に呼び出される
        /// </summary>
        private static void getCurrentRegDateTime()
        {
            // 現実時間::開始日時 2023/6/14 12:00:00 (=regDate 1/1/1 06:00:00)
            DateTime dtSince = new DateTime(2023, 6, 14, 12, 00, 00);
            // 現実時間::現在日時 DateTime,Now
            DateTime dtNow = DateTime.Now;


            TimeSpan dif = dtNow - dtSince;

            TimeSpan difInside = dif * 28.8;

            regDay = difInside.Days;       // これまでの総日数 使用しない

            // 分→時への繰り上がりを考慮するため、小さい位を先に計算する
            regMinute = difInside.Minutes + 34;

            regHour = difInside.Hours + 14; // リリース開始時が内部時間の6時だったため

            // 秒は使わないので順不同
            //regSecond = difInside.Seconds;
        }

        /// <summary>
        /// 現実時間 :: タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = GetNow().ToString("yyyy/MM/dd") /*GetNow().ToLongDateString()*/ + " (" + GetDayOfWeekJp(GetNow().DayOfWeek) + ") " + GetNow().ToLongTimeString();

            // +++ ラウェハラ潮時間表示機能に関する実装 +++ //

            // 内部時間を都度取得する (潮時間や次回レイド時間の基準判別用)  
            DateTime regDateTime = GetRegNow(regDay, regHour, regMinute);

            /*
             * ラウェハラ潮時間に関する記述メモ
             * 干潮  2:10 -  8:10 頃
             * 満潮  8:10 - 14:10 頃
             * 干潮 14:10 - 20:10 頃
             * 満潮 20:10 - 翌 2:10 頃
             */

            if ((regDateTime.Hour == 2 && regDateTime.Minute >= 10)
                || (regDateTime.Hour > 2 && regDateTime.Hour < 8)
                || (regDateTime.Hour == 8 && regDateTime.Minute < 10))
            {
                // 以下の条件のいずれかを満たす場合この分岐内へ
                //  * 2時10分(含む)     以降
                //  * 3時台～7時台
                //  * 8時10分(含まない) 以前
                // この条件により、～2:09 と 8:10～ は除外

                // Phase1 内部時間 2:10 - 8:09
                // 現フェーズに初めて突入した時にのみ、現フェーズ終了予定時刻を取得
                if (currentPhase != 1)
                {
                    currentPhase = 1;
                    txtMsg = txt_CurrentSeaLv_Empty; // 干潮
                    groupBox3.BackColor = Color.PeachPuff;
                    seaLvRemainTime = getCurrentSeaLvRemainTime(1, regDateTime.Hour, regDateTime.Minute);
                }
                // 上記以外の中間時間は取得処理を行わない
            }
            else if ((regDateTime.Hour == 8 && regDateTime.Minute >= 10)
                || (regDateTime.Hour > 8 && regDateTime.Hour < 14)
                || (regDateTime.Hour == 14 && regDateTime.Minute < 10))
            {
                // 以下の条件のいずれかを満たす場合この分岐内へ
                //  * 8時10分(含む)     以降
                //  * 9時台～13時台
                //  * 14時10分(含まない) 以前
                // この条件により、～8:09 と 14:10～ は除外

                // Phase2 内部時間 8:10 - 14:09
                // 現フェーズに初めて突入した時のみ、現フェーズ終了予定時刻を取得
                if (currentPhase != 2)
                {
                    currentPhase = 2;
                    txtMsg = txt_CurrentSeaLv_Full; // 満潮
                    groupBox3.BackColor = Color.Aquamarine;
                    seaLvRemainTime = getCurrentSeaLvRemainTime(2, regDateTime.Hour, regDateTime.Minute);
                }
                // 上記以外の中間時間は取得処理を行わない
            }
            else if ((regDateTime.Hour == 14 && regDateTime.Minute >= 10)
                || (regDateTime.Hour > 14 && regDateTime.Hour < 20)
                || (regDateTime.Hour == 20 && regDateTime.Minute < 10))
            {
                // 以下の条件のいずれかを満たす場合この分岐内へ
                //  * 14時10分(含む)     以降
                //  * 15時台～19時台
                //  * 20時10分(含まない) 以前
                // この条件により、～14:09 と 20:10～ は除外

                // phase3 内部時間 14:10 - 20:09
                // 現フェーズに初めて突入した時のみ、現フェーズ終了予定時刻を取得
                if (currentPhase != 3)
                {
                    currentPhase = 3;
                    txtMsg = txt_CurrentSeaLv_Empty; // 干潮
                    groupBox3.BackColor = Color.PeachPuff;
                    seaLvRemainTime = getCurrentSeaLvRemainTime(3, regDateTime.Hour, regDateTime.Minute);
                }
                // 上記以外の中間時間は取得処理を行わない
            }
            else if ((regDateTime.Hour == 20 && regDateTime.Minute >= 10)
                || ((regDateTime.Hour > 20 && regDateTime.Hour < 24)
                    || (regDateTime.Hour >= 0 && regDateTime.Hour < 2))
                || (regDateTime.Hour == 2 && regDateTime.Minute < 10))
            {
                // phase4 内部時間 20:10 - 翌 2:09 ( 20:10-23:59 / 0:00-2:09 )
                // 現フェーズに初めて突入した時のみ、現フェーズ終了予定時刻を取得
                if (currentPhase != 4)
                {
                    // 以下の条件のいずれかを満たす場合この分岐内へ
                    //  * 20時10分(含む)     以降
                    //  * 21時台～23時台 or 0時台～ 1時台
                    //  *  2時10分(含まない) 以前
                    // この条件により、～20:09 と 2:10～ は除外

                    currentPhase = 4;
                    txtMsg = txt_CurrentSeaLv_Full; // 満潮
                    groupBox3.BackColor = Color.Aquamarine;
                    seaLvRemainTime = getCurrentSeaLvRemainTime(4, regDateTime.Hour, regDateTime.Minute);
                }
                // 上記以外の中間時間は取得処理を行わない
            }

            if (seaLvRemainTime.Minute > 0)
            {
                // 残り0分以上の場合 (1分00秒～12分30秒)

                if (seaLvRemainTime.Second > 0)
                {
                    // 残り00秒以上の場合、表示秒数は-1秒
                    seaLvRemainTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, seaLvRemainTime.Minute, seaLvRemainTime.Second - 1);
                }
                else
                {
                    // 残り0秒の場合、秒を59、分を-1
                    seaLvRemainTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, seaLvRemainTime.Minute - 1, 59);
                }
            }
            else
            {
                // 残り0分xx秒の場合 (0分00秒～0分59秒)

                if (seaLvRemainTime.Second > 0)
                {
                    // 残り00秒以上の場合 (0分01秒～0分59秒)
                    seaLvRemainTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, seaLvRemainTime.Minute, seaLvRemainTime.Second - 1);
                }
                else
                {
                    seaLvRemainTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
                }
            }

            groupBox3.Text = txtMsg;
            label3.Text = seaLvRemainTime.ToString("mm:ss");

            // --- レイド･防衛戦開始時間判定処理 --- //

            nextRaidDate = getNextRaidTime(GetNow());

            if ("" == raidOnGoingMsg)
            {
                // レイド･防衛戦 開催中文言が空欄の場合、次回時刻を表示
                label4.Text = nextRaidDate.ToString("yyyy/MM/dd") + " (" + GetDayOfWeekJp(nextRaidDate.DayOfWeek) + ") " + nextRaidDate.ToLongTimeString();
            }
            else
            {
                // 同文言が空欄ではない場合、文言を表示
                label4.Text = raidOnGoingMsg;
            }

            tm1.Elapsed += (sender, e) =>
            {

            };

            tm1.Start();
        }

        /// <summary>
        /// 現在の現実時間から、次回レイド･防衛戦の開始時刻を現実時間で取得
        /// 　戻り値はDateTime型なので、現在レイド中かどうかは、呼び元で別途判別処理を実装すること
        /// </summary>
        /// <param name="currentDate">現在の現実時間</param>
        /// <returns>次回レイド･防衛戦開始時間</returns>
        private DateTime getNextRaidTime(DateTime currentDate)
        {
            DateTime rtnDate = new DateTime();

            if (currentDate.DayOfWeek == DayOfWeek.Sunday || currentDate.DayOfWeek == DayOfWeek.Saturday)
            {
                // 日曜日または土曜日の場合
                // 土日  :  8:00- / 12:00- / 16:00- / 20:00- / 1:00- (土曜未明はなし)

                if (((currentDate.DayOfWeek == DayOfWeek.Saturday && currentDate.Hour >= 0)
                    || (currentDate.DayOfWeek == DayOfWeek.Sunday && currentDate.Hour >= 2))
                    && currentDate.Hour < 8)
                {
                    // 土曜日の0時台～7時台 または 日曜日の2時台～7時台の場合、次回は 8:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 8, 00, 00);
                }
                else if (currentDate.Hour > 8 && currentDate.Hour < 12)
                {
                    // 9時台～11時台の場合、次回は12:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 00, 00);
                }
                else if (currentDate.Hour > 12 && currentDate.Hour < 16)
                {
                    // 13時台～15時台の場合、次回は16:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 00, 00);
                }
                else if (currentDate.Hour > 16 && currentDate.Hour < 20)
                {
                    // 17時台～19時台の場合、次回は20:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 20, 00, 00);
                }
                else if (currentDate.Hour > 20 && currentDate.Hour < 24)
                {
                    // 21時台～23時台の場合、次回は翌日1:00～ (土日共通)
                    // 0:00～0:59については、土曜夜(日曜未明)は次の条件分岐、日曜夜(月曜未明)は平日側で判別
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 1, 1, 00, 00);
                }
                else if (currentDate.DayOfWeek == DayOfWeek.Sunday && currentDate.Hour == 0)
                {
                    // 土曜夜(日曜未明) の 0時台の場合、次のレイドは 1:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 1, 00, 00);
                }

                //  [土日区分でのレイド時間] 土曜日 8/12/16/20 または 日曜日 1/8/16/20 のみ レイド開催中
                if ((currentDate.DayOfWeek == DayOfWeek.Saturday
                    && (currentDate.Hour == 8 || currentDate.Hour == 12 || currentDate.Hour == 16 || currentDate.Hour == 20))
                 || (currentDate.DayOfWeek == DayOfWeek.Sunday
                    && (currentDate.Hour == 1 || currentDate.Hour == 8 || currentDate.Hour == 12 || currentDate.Hour == 16 || currentDate.Hour == 20)))
                {
                    raidOnGoingMsg = "現在 レイド・防衛戦 時間です";
                }
            }
            else
            {
                // 上記以外 (月曜日～金曜日) の場合
                // 月～金: 14:00- / 18:00- / 22:00-

                if (currentDate.DayOfWeek == DayOfWeek.Monday && currentDate.Hour == 0)
                {
                    // 月曜日の0時台の場合、次回は 1:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 1, 00, 00);
                }

                else if (((currentDate.DayOfWeek == DayOfWeek.Monday && currentDate.Hour > 1)
                         || (currentDate.DayOfWeek != DayOfWeek.Monday && currentDate.Hour >= 0))
                         && currentDate.Hour < 14)
                {
                    // 月曜日の1時台～13時台 または 月曜日以外の0時台～13時台の場合、次回は14時
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 00, 00);
                }
                else if (currentDate.Hour > 14 && currentDate.Hour < 18)
                {
                    // 13時台～17時台の場合、次回は18:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 18, 00, 00);
                }
                else if (currentDate.Hour > 18 && currentDate.Hour < 22)
                {
                    // 19時台～21時台の場合、次回は22:00～
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 22, 00, 00);
                }

                // 22時以降の処理は、金曜日か否かで異なる

                if (currentDate.DayOfWeek == DayOfWeek.Friday && currentDate.Hour == 23)
                {
                    // 金曜日の23時台～の場合、次回レイドは土曜日の朝8時 (土曜未明1時ではないので注意!!)
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 1, 9, 00, 00);
                }
                else if (currentDate.DayOfWeek != DayOfWeek.Friday && currentDate.Hour == 23)
                {
                    // 金曜日"以外"の23時台の場合、次回レイドは翌日の14時
                    rtnDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 1, 14, 00, 00);
                }

                // [平日区分でのレイド時間] 月曜日 1/14/18/22 または 月曜日以外 14/18/22 のみレイド開催中
                if ((currentDate.DayOfWeek == DayOfWeek.Monday
                    && (currentDate.Hour == 1 || currentDate.Hour == 14 || currentDate.Hour == 18 || currentDate.Hour == 22))
                 || (currentDate.DayOfWeek != DayOfWeek.Monday 
                    && (currentDate.Hour == 14 || currentDate.Hour == 18 || currentDate.Hour == 22)))
                {
                    raidOnGoingMsg = "現在 レイド・防衛戦 時間です";
                }
            }
            return rtnDate;
        }

        /// <summary>
        /// 現在の潮時間の、現実時間での残り時間を計算する
        /// 　内部時間の時と分を秒に統合し、28.8で割った値が、現実で経過を要する秒数
        /// 　各フェーズ毎に分岐し、現フェーズの終了予定時刻(内部時間)を28.8倍し、
        /// 　現実時間に加算する。(加算前に、時/分/秒の桁上り処理はしておく。
        /// 　※桁上り処理は将来的に外部メソッド化しても良いかもしれないが、とりあえず都度書く
        /// </summary>
        /// <param name="currentPhase">現在のフェーズ (1～4)</param>
        /// <param name="regHour">内部時間の時 (0～23)</param>
        /// <param name="regMinute">内部時間の分 (0～59)</param>
        /// <returns>現実時間での残り時間 (12分30秒以内))</returns>
        private DateTime getCurrentSeaLvRemainTime(int currentPhase, int regHour, int regMinute)
        {
            // (内部時間) 現フェーズ残り時間 (時)
            int regRemainHour = 0;
            // (内部時間) 現フェーズ残り時間 (分)
            int regRemainMinute = 0;
            // (内部時間) 現フェーズ残り時間 (合計秒数換算)
            int regRemainTotalSec = 0;
            // (現実時間) 残り時間の合計秒数 regRemainTotalSec * 28.8
            int remainTotalSec = 0;

            // (現実時間) 残り分数を算出する上で必要になる、小数部ありの分数
            double remainMinuteDouble = 0;

            // (現実時間) 残り時間 (分) 0～12
            int remainMinute = 0;
            // (現実時間) 残り時間 (秒) 0～59
            int remainSec = 0;

            // 共通変換処理
            if (regHour >= 24)
            {
                regHour -= 24;
            }

            if (currentPhase == 1)
            {
                // phase 1 : 2:00 - 7:59 phaseLimit = 8:00

                // (内部時間) 現フェーズ残り時間 (時)
                regRemainHour = 8 - regHour;
                // (内部時間) 現フェーズ残り時間 (分)
                regRemainMinute = 60 - regMinute;

                if (regRemainMinute == 60)
                {
                    // 60 - 0 = 60 の場合は 0 分にする
                    regRemainMinute = 0;
                }

                // (内部時間) 現フェーズ残り時間の合計秒数
                regRemainTotalSec = regRemainHour * 3600 + regRemainMinute * 60;

                // (現実時間) 残り合計秒数を計算、小数部はすべて切り上げする
                // 28.8で割る前の値(=内部時間の合計秒数)が28.8で割り切れなければ、すべてint値で+1、
                // 割り切れるならばそのままのint値を使用する
                if (regRemainTotalSec % 28.8 == 0)
                {
                    // 内部時間が5で割り切れる場合は、現実時間の秒数は小数部切り捨て
                    remainTotalSec = (int)(regRemainTotalSec / 28.8);
                }
                else
                {
                    // 上記以外、内部時間が5で割り切れない場合、現実時間の秒数は一律小数部切り上げ
                    remainTotalSec = (int)(regRemainTotalSec / 28.8) + 1;
                }

                // 現実時間の残り秒数を60で割って、残りの分数を算出
                // 割り切れない場合は小数部第2位までを変数に移し替えて、60倍して秒数を出す
                if (remainTotalSec % 60 == 0)
                {
                    // ピッタリ60秒で割り切れる場合、その値を戻り値の分に格納
                    remainMinute = remainTotalSec / 60;
                    // 秒は0秒
                    remainSec = 0;
                }
                else
                {
                    // (現実時間)残り合計秒数が60で割り切れない場合、
                    // 端数の秒数が存在するので、残り分を抽出後、端数をdouble型の仮変数に格納
                    // その後60倍して秒数を求める (端数切り捨て)
                    remainMinuteDouble = (double)remainTotalSec / 60;

                    // 戻り値 (分) 小数部の端数切り捨て
                    remainMinute = (int)remainMinuteDouble;
                    // 戻り値 (秒) 上記端数を60倍したもの
                    remainSec = (int)((double)(remainMinuteDouble - remainMinute) * 60);
                }
            }
            else if (currentPhase == 2)
            {
                // phase 2 : 8:00 - 13:59 phaseLimit = 14:00

                // (内部時間) 現フェーズ残り時間 (時)
                regRemainHour = 14 - regHour;
                // (内部時間) 現フェーズ残り時間 (分)
                regRemainMinute = 60 - regMinute;

                if (regRemainMinute == 60)
                {
                    // 60 - 0 = 60 の場合は 0 分にする
                    regRemainMinute = 0;
                }

                // (内部時間) 現フェーズ残り時間の合計秒数
                regRemainTotalSec = regRemainHour * 3600 + regRemainMinute * 60;

                // (現実時間) 残り合計秒数を計算、小数部はすべて切り上げする
                // 28.8で割る前の値(=内部時間の合計秒数)が28.8で割り切れなければ、すべてint値で+1、
                // 割り切れるならばそのままのint値を使用する
                if (regRemainTotalSec % 28.8 == 0)
                {
                    // 内部時間が5で割り切れる場合は、現実時間の秒数は小数部切り捨て
                    remainTotalSec = (int)(regRemainTotalSec / 28.8);
                }
                else
                {
                    // 上記以外、内部時間が5で割り切れない場合、現実時間の秒数は一律小数部切り上げ
                    remainTotalSec = (int)(regRemainTotalSec / 28.8) + 1;
                }

                // 現実時間の残り秒数を60で割って、残りの分数を算出
                // 割り切れない場合は小数部第2位までを変数に移し替えて、60倍して秒数を出す
                if (remainTotalSec % 60 == 0)
                {
                    // ピッタリ60秒で割り切れる場合、その値を戻り値の分に格納
                    remainMinute = remainTotalSec / 60;
                    // 秒は0秒
                    remainSec = 0;
                }
                else
                {
                    // (現実時間)残り合計秒数が60で割り切れない場合、
                    // 端数の秒数が存在するので、残り分を抽出後、端数をdouble型の仮変数に格納
                    // その後60倍して秒数を求める (端数切り捨て)
                    remainMinuteDouble = (double)remainTotalSec / 60;

                    // 戻り値 (分) 小数部の端数切り捨て
                    remainMinute = (int)remainMinuteDouble;
                    // 戻り値 (秒) 上記端数を60倍したもの
                    remainSec = (int)((double)(remainMinuteDouble - remainMinute) * 60);
                }

            }
            else if (currentPhase == 3)
            {
                // phase 3 ; 14:00 - 19:59 phaseLimit = 20:00

                // (内部時間) 現フェーズ残り時間 (時)
                regRemainHour = 20 - regHour;
                // (内部時間) 現フェーズ残り時間 (分)
                regRemainMinute = 60 - regMinute;

                if (regRemainMinute == 60)
                {
                    // 60 - 0 = 60 の場合は 0 分にする
                    regRemainMinute = 0;
                }

                // (内部時間) 現フェーズ残り時間の合計秒数
                regRemainTotalSec = regRemainHour * 3600 + regRemainMinute * 60;

                // (現実時間) 残り合計秒数を計算、小数部はすべて切り上げする
                // 28.8で割る前の値(=内部時間の合計秒数)が28.8で割り切れなければ、すべてint値で+1、
                // 割り切れるならばそのままのint値を使用する
                if (regRemainTotalSec % 28.8 == 0)
                {
                    // 内部時間が5で割り切れる場合は、現実時間の秒数は小数部切り捨て
                    remainTotalSec = (int)(regRemainTotalSec / 28.8);
                }
                else
                {
                    // 上記以外、内部時間が5で割り切れない場合、現実時間の秒数は一律小数部切り上げ
                    remainTotalSec = (int)(regRemainTotalSec / 28.8) + 1;
                }

                // 現実時間の残り秒数を60で割って、残りの分数を算出
                // 割り切れない場合は小数部第2位までを変数に移し替えて、60倍して秒数を出す
                if (remainTotalSec % 60 == 0)
                {
                    // ピッタリ60秒で割り切れる場合、その値を戻り値の分に格納
                    remainMinute = remainTotalSec / 60;
                    // 秒は0秒
                    remainSec = 0;
                }
                else
                {
                    // (現実時間)残り合計秒数が60で割り切れない場合、
                    // 端数の秒数が存在するので、残り分を抽出後、端数をdouble型の仮変数に格納
                    // その後60倍して秒数を求める (端数切り捨て)
                    remainMinuteDouble = (double)remainTotalSec / 60;

                    // 戻り値 (分) 小数部の端数切り捨て
                    remainMinute = (int)remainMinuteDouble;
                    // 戻り値 (秒) 上記端数を60倍したもの
                    remainSec = (int)((double)(remainMinuteDouble - remainMinute) * 60);
                }
            }
            else if (currentPhase == 4)
            {
                // phase 4 ; 20:00 - 23;59 or 0:00 - 1:59 phaseLimit = 2:00

                // (内部時間) 現フェーズ残り時間 (時) - phase4だけは24時前後で分岐する
                if (regHour >= 20 && regHour < 24)
                {
                    // 20時台～23時台
                    regRemainHour = (23 - regHour) + 2;
                }
                else
                {
                    // 0時台～1時台
                    regRemainHour = 2 - regHour;
                }

                // (内部時間) 現フェーズ残り時間 (分)
                regRemainMinute = 60 - regMinute;

                if (regRemainMinute == 60)
                {
                    // 60 - 0 = 60 の場合は 0 分にする
                    regRemainMinute = 0;
                }

                // (内部時間) 現フェーズ残り時間の合計秒数
                regRemainTotalSec = regRemainHour * 3600 + regRemainMinute * 60;

                // (現実時間) 残り合計秒数を計算、小数部はすべて切り上げする
                // 28.8で割る前の値(=内部時間の合計秒数)が28.8で割り切れなければ、すべてint値で+1、
                // 割り切れるならばそのままのint値を使用する
                if (regRemainTotalSec % 28.8 == 0)
                {
                    // 内部時間が5で割り切れる場合は、現実時間の秒数は小数部切り捨て
                    remainTotalSec = (int)(regRemainTotalSec / 28.8);
                }
                else
                {
                    // 上記以外、内部時間が5で割り切れない場合、現実時間の秒数は一律小数部切り上げ
                    remainTotalSec = (int)(regRemainTotalSec / 28.8) + 1;
                }

                // 現実時間の残り秒数を60で割って、残りの分数を算出
                // 割り切れない場合は小数部第2位までを変数に移し替えて、60倍して秒数を出す
                if (remainTotalSec % 60 == 0)
                {
                    // ピッタリ60秒で割り切れる場合、その値を戻り値の分に格納
                    remainMinute = remainTotalSec / 60;
                    // 秒は0秒
                    remainSec = 0;
                }
                else
                {
                    // (現実時間)残り合計秒数が60で割り切れない場合、
                    // 端数の秒数が存在するので、残り分を抽出後、端数をdouble型の仮変数に格納
                    // その後60倍して秒数を求める (端数切り捨て)
                    remainMinuteDouble = (double)remainTotalSec / 60;

                    // 戻り値 (分) 小数部の端数切り捨て
                    remainMinute = (int)remainMinuteDouble;
                    // 戻り値 (秒) 上記端数を60倍したもの
                    remainSec = (int)((double)(remainMinuteDouble - remainMinute) * 60);
                }
            }

            // 本メソッド参照元は、分と秒以外は参照しない前提なので、適当な値でかまわない
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, remainMinute, remainSec);
        }

        /// <summary>
        /// 現在の日時を取得するメソッド
        /// </summary>
        /// <returns></returns>
        private static DateTime GetNow()
        {
            return DateTime.Now;
        }

        private static DateTime GetRegNow(int day, int hour, int minute)
        {
            if (minute >= 60)
            {
                hour++;
                minute -= 60;
            }

            if (hour >= 24)
            {
                hour -= 24;
            }
            // ここでは年月日に現実時間を入れているが、実際参照しないし表示もしないので良しとする
            // 将来的に内部時間の年月日を導入する場合は、うまく入れ替えてやる必要がある。
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 00);
        }

        /// <summary>
        /// 曜日の和名を返すメソッド
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private static string GetDayOfWeekJp(DayOfWeek dayOfWeek)
        {
            string str = "";

            if (dayOfWeek == DayOfWeek.Sunday)
            {
                str = "日曜日";
            }
            else if (dayOfWeek == DayOfWeek.Monday)
            {
                str = "月曜日";
            }
            else if (dayOfWeek == DayOfWeek.Tuesday)
            {
                str = "火曜日";
            }
            else if (dayOfWeek == DayOfWeek.Wednesday)
            {
                str = "水曜日";
            }
            else if (dayOfWeek == DayOfWeek.Thursday)
            {
                str = "木曜日";
            }
            else if (dayOfWeek == DayOfWeek.Friday)
            {
                str = "金曜日";
            }
            else if (dayOfWeek == DayOfWeek.Saturday)
            {
                str = "土曜日";
            }
            return str;
        }

        /// <summary>
        /// 内部時間 :: タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            // 時間帯によって表示を切り替える、太陽と月のアイコン(機種依存文字)
            string cycleIcon = "";

            DateTime regDateTime = GetRegNow(regDay, regHour, regMinute);

            if (regDateTime.Hour >= 6 && regDateTime.Hour <= 17)
            {
                // (24時間表記で) 6時台から17時台まで、昼
                // ※内部時間は現実秒数を28.8倍する都合上、分や秒の繰り上がりが生じる場合がある
                // 複雑な条件分岐を毎回書くのは面倒なので、GetRegNowでそれらを処理した後のDateTime型変数を参照する
                cycleIcon = "昼 ";
                groupBox2.BackColor = Color.Moccasin;
                groupBox2.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
            }
            else
            {
                // 上記時間帯以外、夜
                // 上記以外の時間帯はすべて夜
                cycleIcon = "夜 ";
                groupBox2.BackColor = Color.DarkSlateBlue;
                groupBox2.ForeColor = Color.White;
                label2.ForeColor = Color.White;
            }

            // 以下年月日は未実装 (処理が煩雑)
            // /*GetRegNow(regDay, regHour, regMinute).ToLongDateString() +*/
            label2.Text = cycleIcon + GetRegNow(regDay, regHour, regMinute).ToString("HH:mm");

            // label2.Text について
            // DateTime.Nowの自動処理を、手動で以下に実装する。

            tm2.Elapsed += (sender, e) =>
            {
                // 秒ごとに処理をする場合はここに記述

                // 1秒毎に現実時間の最新時間を取得し、つどリリース日時との差分を取る
                getCurrentRegDateTime();
            };
            tm2.Start();
        }

        /// <summary>
        /// 消耗品タイマー用 Tickイベントメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            // タイマー起動時の表示減算処理をここに書く
            int tickHour = consumeTimer.Hour;
            int tickMinute = consumeTimer.Minute;
            int tickSecond = consumeTimer.Second;

            if (consumeTimer.Hour != 0)
            {
                // 単位"時間"0以外
                if (consumeTimer.Minute == 0)
                {
                    // 単位"分"0
                    if (consumeTimer.Second == 0)
                    {
                        // (例外) 時間 桁下がり処理
                        // 単位"秒"0
                        // 例) 2:00:00 -> 1:59:59 / 1:00:00 -> 0:59:59
                        tickHour -= 1;
                        tickMinute = 59;
                        tickSecond = 59;
                    }
                    else
                    {
                        // 単位"秒"0以外
                        // 例) 2:00:01 -> 2:00:00 / 1:00:59 -> 1:00:58
                        tickSecond -= 1;
                    }
                }
                else
                {
                    // 単位"分"0以外
                    if (consumeTimer.Second == 0)
                    {
                        // 単位"秒"0
                        // 例) 2:01:00 -> 2:00:59 / 1:59:00 -> 1:58:59
                        tickMinute -= 1;
                        tickSecond = 59;
                    }
                    else
                    {
                        // 単位"秒"0以外
                        // 例) 2:01:01 -> 2:01:00 / 1:59:59 -> 1:59:58
                        tickSecond -= 1;
                    }
                }
            }
            else
            {
                // 単位"時間"0
                if (consumeTimer.Minute == 0)
                {
                    // 単位"分"0
                    if (consumeTimer.Second == 0)
                    {
                        // 単位"秒"0
                        // 例) 0:00:00 ... 時間切れ、文字色を赤と黒で反転
                        if (label5.ForeColor == Color.Black)
                        {
                            label5.ForeColor = Color.Red;
                        }
                        else
                        {
                            label5.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        // 単位"秒"0以外
                        // 例) 0:00:05 -> 0:00:04 / 0:00:01 -> 0:00:00
                        tickSecond -= 1;
                    }
                }
                else
                {
                    // 単位"分"0以外
                    if (consumeTimer.Second == 0)
                    {
                        // 単位"秒"0
                        // 例) 0:01:00 -> 0:00:59
                        tickMinute -= 1;
                        tickSecond = 59;
                    }
                    else
                    {
                        // 単位"秒"0以外
                        // 例) 0:01:01 -> 0:01:00
                        tickSecond -= 1;
                    }
                }
            }

            consumeTimer = new DateTime(GetNow().Year, GetNow().Month, GetNow().Day, tickHour, tickMinute, tickSecond);

            label5.Text = consumeTimer.ToString("HH:mm:ss");

            tm3.Elapsed += (sender, e) =>
            {

            };
            tm3.Start();
        }

        /// <summary>
        /// 消耗品タイマー :: 30m ボタン押下時処理
        /// 　既に起動中の消耗品タイマーを停止(リセット)し、新たに30分のタイマーを起動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 既に起動中のものがあれば停止する
            timer3.Stop();
            // ラベルの表示を初期化
            consumeTimer = new DateTime(GetNow().Year, GetNow().Month, GetNow().Day, 00, 30, 00);
            label5.Text = consumeTimer.ToString("HH:mm:ss");
            // タイマー再起動
            timer3.Start();
        }

        /// <summary>
        /// 消耗品タイマー :: 60m ボタン押下時処理
        /// 　既に起動中の消耗品タイマーを停止(リセット)し、新たに30分のタイマーを起動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 既に起動中のものがあれば停止する
            timer3.Stop();
            // ラベルの表示を初期化
            consumeTimer = new DateTime(GetNow().Year, GetNow().Month, GetNow().Day, 01, 00, 00);
            label5.Text = consumeTimer.ToString("HH:mm:ss");
            // タイマー再起動
            timer3.Start();
        }

        /// <summary>
        /// 消耗品タイマー :: 180m ボタン押下時処理
        /// 　既に起動中の消耗品タイマーを停止(リセット)し、新たに30分のタイマーを起動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            // 既に起動中のものがあれば停止する
            timer3.Stop();
            // ラベルの表示を初期化
            consumeTimer = new DateTime(GetNow().Year, GetNow().Month, GetNow().Day, 03, 00, 00);
            label5.Text = consumeTimer.ToString("HH:mm:ss");
            // タイマー再起動
            timer3.Start();
        }

        /// <summary>
        /// 消耗品タイマー :: 初期化 ボタン押下時処理
        /// 　既に起動中の消耗品タイマーを停止(リセット)する。再起動はしない。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            // 既に起動中のものがあれば停止する
            timer3.Stop();
            // ラベル表示を初期化 (**:**:**)
            label5.Text = "**:**:**";
            label5.ForeColor = Color.Black;
        }
    }
}
