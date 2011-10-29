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
using System.Collections.Generic;

namespace kosoku
{
    public class Data_Route : IComparable<Data_Route>
    {
        // data define for XML
        public int routeNo { get; set; }// RouteNo 経路番号
        // Summary 検索結果の概要(別表参照)
        private int totalToll;
        // TotalToll 料金の合計
        public int TotalToll
        {
            get
            {
                int output = 0;

                foreach (Data_Section ds in details)
                {
                    output += ds.tolls[ds.selectedToll].price;
                }

                return output;
            }
            set { this.totalToll = value; }
        }
        public string stotalToll { get { return TotalToll + "円"; } }

        public int totalTime { get; set; }// TotalTime 所要時間の合計(分)
        public string stotalTime
        {
            get
            {
                if (totalTime < 60)
                {
                    return totalTime + "分";
                }
                else
                {
                    return totalTime / 60 + "時間" + totalTime % 60 + "分";
                }
            }
        }
       
        public float totalLength { get; set; }// TotalLength 距離の合計
        public string stotalLength { get { return totalLength + "km"; } }
        public List<Data_Section> details = new List<Data_Section>(); // Details
        public List<Data_Section> Details { get { return this.details; } set { this.details = value; } }

        // variable define for running
        public int sortby = 0;




        //public int compareTo(Data_Route another) {
        int IComparable<Data_Route>.CompareTo(Data_Route another)
        {
            int re = 0;
            switch (sortby)
            {
                case 0:// time
                    re = this.totalTime - another.totalTime;
                    break;
                case 1:// Length
                    re = (int)(this.totalLength - another.totalLength);
                    break;
                case 2:// price
                    re = this.totalToll - another.totalToll;
                    break;
            }
            return re;
        }

    }
}
