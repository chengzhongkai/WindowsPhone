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
    public class Data_SubSection    
    {
        // var def for xml
        public string from { get; set; } // From 出発IC名
        public string to { get; set; } // To 到着IC名
        public string road { get; set; } // Road 道路名
        public float length { get; set; } // Length 距離(km)
        public int time { get; set; } // Time 時間(分)

        public string route
        {
            get
            {
                string rt = from + "→" + to;
                if (rt.Length < 15)
                    return rt;
                else
                    return rt.Substring(0, 15) + "...";
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



    }
}
