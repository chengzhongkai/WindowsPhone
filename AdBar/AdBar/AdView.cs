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
using System.Runtime.Serialization.Json;
using System.IO;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;



namespace AdBar
{
    public class AdView : UserControl
    {
        /** SDKのバージョン */
        public static String VERSION = "1.0.0";

        public event RoutedEventHandler Click;
        private DispatcherTimer tmr;

        private MarqueeText mTxt;

        private Image img;
        private Grid grd;

        public string mPublisherID { get; set; }
        private int curAd = -1;
        private Result adList;
        private string clickUrl = "";


        EventHandler enh;

        private void init()
        {
            this.MouseLeftButtonUp += new MouseButtonEventHandler(AdBar_MouseLeftButtonUp);
            this.Loaded += new RoutedEventHandler(AdView_Loaded);

        }

        void AdView_Loaded(object sender, RoutedEventArgs e)
        {
            if (double.IsNaN(this.Height))
            {
                this.Height = 78;
            }

            if (double.IsNaN(this.Width))
            {
                this.Width = 480;
            }


            mTxt = new MarqueeText(this.Width, this.Height);


            img = new Image();
            img.Width = base.Width;
            img.Height = base.Height;

            grd = new Grid();

            grd.Width = base.Width;
            grd.Height = base.Height;

            grd.Children.Add(mTxt);
            grd.Children.Add(img);

            this.Content = grd;

            tmr = new DispatcherTimer();

            loadAd();
        }

        public AdView()
        {
            init();
        }

        public AdView(String id)
        {
            this.mPublisherID = id;
            init();
        }

        /**
         *広告へ遷移
         */
        void AdBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
            {
                Click(this, new RoutedEventArgs());
            }
            else
            {
                // タスクを新しく作成する
                var task = new WebBrowserTask();

                // 広告 のページを表示したい
                task.Uri = new Uri(this.clickUrl, UriKind.Absolute);

                // タスクを実行する
                task.Show();
            }

        }

        /**
         *広告データを取得
         */
        void loadAd()
        {
            WebClient cli = new WebClient();
            Uri uri;
            string url = "http://api.behaviad.net/0.2/" + this.getRequestUrl();

            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                MessageBox.Show("指定されたURIに間違いがあります");
                return;
            }

            cli.OpenReadCompleted += new OpenReadCompletedEventHandler(HandleResponse);

            cli.OpenReadAsync(uri);
        }

        /**
         *広告データを解析
         */
        void HandleResponse(object sender, OpenReadCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                // MessageBox.Show("エラー:" + e.Error.Message);
                return;
            }

            Stream stream = e.Result;


            /* check data
            StreamReader re2 = new StreamReader(stream);
            stream.Position = 0;
            string a2 = re2.ReadToEnd();  */

            DataContractJsonSerializer parse = new DataContractJsonSerializer(typeof(Result));
            Result rs = null;
            try
            {
                rs = (Result)parse.ReadObject(e.Result);
            }
            catch (System.Runtime.Serialization.SerializationException ee)
            {
                string a = ee.Message;
            }

            /*
            Ads ad = new Ads();
            ad.id = 2231;
            ad.type = "image";
            ad.clickUrl = "http://api.behaviad.net/0.2/r/?id=1&ad=2231&sid=mi6gaplti8gsgq31bjkql1a217";
            ad.imageUrl = "http://img.behaviad.net/bnr/320x48/1314074148_0.png";
            ad.refreshInterval=10;
            rs.ads = new[] {ad,rs.ads[0] };
            rs.length = 2;
            */

            if (rs != null && rs.length > 0)
            {
                this.adList = rs;
                this.curAd = 0;
                showAd(this.adList.ads[0]);
            }

        }

        /**
         *広告バーを表す
         */
        void showAd(Ads ads)
        {
            if (ads.type.Equals("text"))
            {
                this.mTxt.Visibility = Visibility.Visible;
                this.img.Visibility = Visibility.Collapsed;

                this.clickUrl = ads.clickUrl;

                this.mTxt.MarqueeContent = ads.text;
                this.mTxt.Foreground = new SolidColorBrush(ads.getTextColor());
                this.mTxt.MarqueeBackground = new SolidColorBrush(ads.getBackgroundColor());

            }
            else if (ads.type.Equals("image"))
            {
                this.mTxt.Visibility = Visibility.Collapsed;
                this.img.Visibility = Visibility.Visible;

                this.clickUrl = ads.clickUrl;

                this.img.Source = new BitmapImage(new Uri(ads.imageUrl, UriKind.RelativeOrAbsolute));

            }

            // show next ad 
            if (this.curAd == this.adList.length - 1)
            {
                this.curAd = 0;
            }
            else
            {
                this.curAd++;
            }

            tmr.Interval = TimeSpan.FromSeconds(ads.refreshInterval);
            enh = new EventHandler(tmr_Tick);
            tmr.Tick += enh;
            tmr.Start();



        }

        /**
         *次の広告を表示
         */
        void tmr_Tick(object sender, EventArgs e)
        {
            tmr.Stop();
            tmr.Tick -= enh;
            showAd(this.adList.ads[this.curAd]);
        }

        /**
         * 広告取得URLを返す
         */
        private String getRequestUrl()
        {
            String url = "?id=" + mPublisherID;
            String time = "" + GetCurrentMilliseconds();
            //url += "&time=" + time;
            url += "&dt=mobile_app";
            url += "&sv=" + VERSION;
            url += "&s=320x48";

            // url += "&ua="+HttpUtility.UrlEncode("Mozilla/4.0 (compatible; MSIE 7.0; Windows Phone OS 7.5; Trident/3.1; IEMobile/9.0)");
            url += "&ua=Windows+Phone";



            url += "&hl=ja&n=10";
            // url += "&ai=jp.co.goga";

            // url += "&at=image";

            return url;
        }

        /**
         *システム時間を取得
         */
        public static double GetCurrentMilliseconds()
        {
            DateTime staticDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - staticDate;
            return timeSpan.TotalMilliseconds;
        }



    }
}

