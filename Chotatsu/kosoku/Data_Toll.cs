using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace kosoku
{

    public enum DaySaleType { DSAll, DSWeekDay, DSHoliday, DSSunDay, DSEtc };
    public enum TimeSaleType { TSAll, TSNoon, TSNight22_6, TSNight0_4, TSNight20_24, TSWork, TSPeek, TSOff, TSEtc };

    public class Data_Toll
    {
        //variable list
        public int tollid { get; set; }

        public string oname="";
        public string oname2="";
        public string name
        {
            get
            {
                string rt = oname;
                if (rt.Length <15)
                    return rt;
                else
                    return rt.Substring(0, 15) + "...";
            }
            set {
                oname = value;
            }
        } // 料金名前
        public int price { get; set; } // 料金
        public string name2
        {
            get
            {
                string rt =oname2;
                if (rt.Length < 15)
                    return rt;
                else
                    return rt.Substring(0, 15) + "...";
            }
            set
            {
                oname2 = value;
            }
        } // 料金名前
        private int prc2 = -2;
        public int price2 { get { return prc2; } set { prc2 = value; } } // 料金


        public string sprice { get { return price + "円"; } } // 料金
        public string sprice2 { get { if (price2 > -1)return price2 + "円"; else return ""; } } // 料金

        public DaySaleType day; // 日割引 0:AllDay 1:平日 2:休日 3:日曜日
        public TimeSaleType time; // 時間割引

        //method list
        /* String分析 */
        void analysisString(string str)
        {
            string[] strList = str.Split(' '); // Space分け 例>平日料金 500円 → 平日料金, 500円
            if (strList.Length < 2)
                return;


            // 500円 例>500円 → 500
            string priceList = strList[0].Substring(0, strList[0].IndexOf("円"));

            if (priceList.Length < 1)
                return;


            this.price = int.Parse(priceList);
            this.name = strList[1];
            this.day = DaySaleType.DSAll;
            this.time = TimeSaleType.TSAll;

            if (str.IndexOf("平日") > -1)
                day = DaySaleType.DSWeekDay;
            if (str.IndexOf("休日") > -1)
                day = DaySaleType.DSHoliday;
            if (str.IndexOf("日曜") > -1)
                day = DaySaleType.DSSunDay;

            if (str.IndexOf("昼間") > -1)
                time = TimeSaleType.TSNoon;
            if (str.IndexOf("夜間") > -1)
                time = TimeSaleType.TSNight22_6;
            if (str.IndexOf("0-4") > -1)
                time = TimeSaleType.TSNight0_4;
            if (str.IndexOf("20-24") > 0)
                time = TimeSaleType.TSNight20_24;
            if (str.IndexOf("通勤") > -1)
                time = TimeSaleType.TSWork;
            if (str.IndexOf("ピーク") > -1)
                time = TimeSaleType.TSPeek;
            if (str.IndexOf("オフ") > -1)
                time = TimeSaleType.TSOff;

        }

        //initialize toll object
        public Data_Toll(string str)
        {
            analysisString(str);

        }



    }
}
