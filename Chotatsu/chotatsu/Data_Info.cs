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

namespace chotatsu
{
    public class Data_Info
    {
        public DateTime bidDeadline{ get; set; }
        public DateTime registryDate { get; set; }
        public string subject { get; set; }
        public string organization{ get; set; }
        public string url{ get; set; }

        public string sbidDeadline { get { return "期限:" + bidDeadline.ToString("yy/MM/dd"); } }
        public string sregistryDate { get { return "登録:" + registryDate.ToString("yy/MM/dd"); } }
    }
}
