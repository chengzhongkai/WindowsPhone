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

namespace chotatsu
{
    public class Data_Result
    {
        public int count { get; set; } // !< 見つかった件数の総計
        public String keyword; // !< 指定したキーワード
        public String organization; // !< 指定した組織名
        public int order; // !< 並び換えの順序
        public int limit; // !< 取得件数
        public int page; // !< 表示開始位置
        public bool isNew { get; set; }
        public List<Data_Info> details; // !< Infoのための配列

        public Data_Result()
        {

            this.details = new List<Data_Info>();
        }
    }
}
