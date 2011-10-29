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
    public class Data_Section
    {
        //Order	区間の順番


        public string subrouteNo { get; set; }// RouteNo 経路番号

        public string from { get; set; }//From	その区間の出発IC名
        public string to { get; set; }//To	その区間の到着IC名
        public float length { get; set; }//Length	その区間の距離(km)
        public List<Data_SubSection> subSections = new List<Data_SubSection>();//SubSections	その区間の詳細(別表参照)
        public List<Data_Toll> tolls = new List<Data_Toll>();//Tolls	料金(別表参照)
        public int time { get; set; }	//Time	所要時間(分)
        private int sltToll = 0;
        public int selectedToll
        {
            get { return this.sltToll; }
            set
            {
                this.tolls[sltToll].name2 = "";
                this.tolls[sltToll].price2 = -2;
                this.sltToll = value;
                this.tolls[sltToll].name2 = this.tolls[sltToll].name;
                this.tolls[sltToll].price2 = this.tolls[sltToll].price;
            }
        }				//選択された料金


        public int price
        {
            // set { this.re = value; }
            get { return this.tolls[selectedToll].price; }
        }


        public string route
        {
            get
            {
                string rt = from + "→" + to;
                if (rt.Length < 11)
                    return rt;
                else
                    return rt.Substring(0, 9) + "...";
            }
        }
        public string slength { get { return length + "km"; } }
        public string stime
        {
            get
            {
                if (time < 60)
                {
                    return time + "分";
                }
                else
                {
                    return time / 60 + "時間" + time % 60 + "分";
                }
            }
        }
        public string sprice { get { return price + "円"; } }
    }
}
