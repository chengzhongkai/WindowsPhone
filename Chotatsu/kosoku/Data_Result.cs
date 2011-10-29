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
    public enum Status { Nothing, NotFound, NotEnd, End }

    public class Data_Result
    {
        public Boolean inited = false;
        public int rid { get; set; }
        public int sid { get; set; }
        public bool isNew { get; set; }
        public List<string> carTypelist = new List<string>();
        public List<string> sortTypelist = new List<string>();
        public List<Data_Route> routes = new List<Data_Route>();

        public String[] carTypes = { "軽自動車等", "普通車", "中型車", "大型車", "特大車" };
        private String[] sortTypes = { "時間", "距離", "価格" };

        public int carType; // 車種
        public string from; // 出発IC名
        public string to; // 到着IC名
        public Boolean loaded = false; // ロード結果 : YES - 成功, NO - 失敗
        public int sortBy; // 並び換えの順序("距離"/"料金")

        // data def for xml
        public Status status = Status.Nothing; // データ結果 : YES - ロート検索成功, NO -

        /** initialize method         */
        public Data_Result()
        {
            for (int i = 0; i < carTypes.Length; i++)
            {
                carTypelist.Add(carTypes[i]);
            }

            for (int i = 0; i < sortTypes.Length; i++)
            {
                sortTypelist.Add(sortTypes[i]);
            }
        }

        /**sort routes*/
        public void sortRoutes()
        {
            foreach (Data_Route route in routes)
            {
                route.sortby = this.sortBy;
            }

            routes.Sort();
        }


        public void clear()
        {

            routes.Clear();
        }

        public void setSubRouteID()
        {
            for (int i = 0; i < this.routes.Count; i++)
            {

                for (int j = 0; j < this.routes[i].Details.Count; j++)
                {
                    this.routes[i].Details[j].subrouteNo = "" + i + ":" + j;


                }

            }

        }

    }
}
