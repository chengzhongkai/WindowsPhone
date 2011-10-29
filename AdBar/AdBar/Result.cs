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

namespace AdBar
{
    public class Result
    {
        public Error error { get; set; }
        public int length { get; set; }

        public Ads[] ads { get; set; }


       
    }
    public class Error {
        public int code { get; set; }
        public string message { get; set; }
    }
    public class Ads
    {
        public int id { get; set; }


        public string type { get; set; }


        public string text { get; set; }


        public string clickUrl { get; set; }

        public string imageUrl { get; set; }

        public int refreshInterval { get; set; }


        public string textColor { get; set; }
        public Color getTextColor()
        {
            return strToColor(this.textColor);

        }

        public string backgroundColor { get; set; }
        public Color getBackgroundColor()
        {
            return strToColor(this.backgroundColor);

        }

        private Color strToColor(string strClr)
        {
            if (strClr.Length == 6)
            {
               
                byte r = Convert.ToByte(strClr.Substring(0, 2),16);
                byte g = Convert.ToByte(strClr.Substring(2, 2),16);
                byte b = Convert.ToByte(strClr.Substring(4, 2),16);
                return Color.FromArgb(255, r, g, b);

            }
            else
            {
                return Colors.Transparent;
            }

        }
    }
}