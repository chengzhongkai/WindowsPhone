using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//susing System.ServiceModel.Syndication;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace kosoku
{

    public partial class result : PhoneApplicationPage
    {

        //Data_Result DRs = new Data_Result();
        Data_Result DRs = null;
        string startIc;
        string endIC;
        string type;
        string[] sortTypes = { "(早)時間順", "(短)距離順", "(安)価格順" };
        public result()
        {
            App app = Application.Current as App;
            if (app.DRs != null)
            {
                this.DRs = app.DRs;
            }
            else
            {
                this.DRs = new Data_Result();
                app.DRs = this.DRs;
            }
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (DRs.isNew)
            {
                this.DRs.isNew = false;

                this.routesList.ItemsSource = null;
                this.routesList.Items.Clear();
                this.DRs.setSubRouteID();
                this.routesList.ItemsSource = this.DRs.routes;
            }
            else
            {
                string value = string.Empty;
                IDictionary<string, string> queryString = this.NavigationContext.QueryString;
                queryString.TryGetValue("c", out value);
                if (value != null)
                {
                    type = value;
                }

                queryString.TryGetValue("f", out value);
                if (value != null)
                {
                    startIc = value;
                }

                queryString.TryGetValue("t", out value);
                if (value != null)
                {
                    endIC = value;
                }


                getrss();
            }

            base.OnNavigatedTo(e);
        }


        void getrss()
        {
            WebClient cli = new WebClient();

            Uri uri;
            string url = "http://kosoku.jp/api/route.php?f=" + startIc + "&t=" + endIC + "&c=" + type;
            //string url = "http://kosoku.jp/api/route.php?f=渋谷&t=浜松&c=普通車";



            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                MessageBox.Show("指定されたURIに間違いがあります");
                return;
            }

            cli.OpenReadCompleted += new
        OpenReadCompletedEventHandler(HandleResponse);
            cli.OpenReadAsync(uri);


        }


        void HandleResponse(object sender, OpenReadCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                // MessageBox.Show("エラー:" + e.Error.Message);
                return;
            }

            Stream stream = e.Result;
            this.DRs.clear();
            try
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    // initialize variable

                    System.DateTime calendar;
                    int gTime;
                    int gDay = 0;

                    calendar = System.DateTime.Now;
                    gTime = calendar.Hour;

                    int day = int.Parse(calendar.DayOfWeek.ToString("d"));
                    if (day == 6)
                        gDay = 1;
                    if (day == 7)
                        gDay = 2;

                    Data_Route currentRoute = null;
                    Data_Section currentSection = null;
                    Data_SubSection currentSubSection = null;
                    string builder = "";
                    DRs.inited = true;
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element: // ノードはエレメントです。


                                if (reader.Name == "Route")
                                {
                                    currentRoute = new Data_Route();
                                    currentRoute.routeNo = DRs.routes.Count;
                                }
                                else if (reader.Name == "Section")
                                {
                                    currentSection = new Data_Section();
                                }
                                else if (reader.Name == "SubSection")
                                {
                                    currentSubSection = new Data_SubSection();
                                }


                                break;
                            case XmlNodeType.Text: // 各エレメントのテキストを表示します。
                                Console.WriteLine(reader.Value);
                                builder = reader.Value;
                                break;
                            case XmlNodeType.EndElement: // エレメントの最後を表示します。




                                if (reader.Name == "Status")
                                {
                                    if (builder == "End")
                                    {
                                        DRs.status = Status.End;
                                    }
                                    else if (builder == "NotEnd")
                                    {
                                        DRs.status = Status.NotEnd;
                                    }
                                }
                                else if (reader.Name == "NotFound")
                                {
                                    DRs.status = Status.NotFound;

                                }
                                else if (reader.Name == "From")
                                {
                                    if (currentSubSection != null)
                                    {
                                        currentSubSection.from = builder;
                                    }
                                    else if (currentSection != null)
                                    {
                                        currentSection.from = builder;
                                    }
                                    else
                                    {
                                        DRs.from = builder;
                                    }

                                }
                                else if (reader.Name == "To")
                                {
                                    if (currentSubSection != null)
                                    {
                                        currentSubSection.to = builder;
                                    }
                                    else if (currentSection != null)
                                    {
                                        currentSection.to = builder;
                                    }
                                    else
                                    {
                                        DRs.to = builder;
                                    }
                                }
                                else if (reader.Name == "CarType")
                                {
                                    DRs.carType = DRs.carTypelist.IndexOf(builder);
                                }
                                else if (reader.Name == "SortBy")
                                {
                                    DRs.sortBy = DRs.sortTypelist.IndexOf(builder);
                                }
                                else if (reader.Name == "Route")
                                {
                                    DRs.routes.Add(currentRoute);
                                    currentRoute = null;
                                }
                                else if (reader.Name == "Routes")
                                {
                                    // result.status = true;
                                }
                                else if (reader.Name == "RouteNo")
                                {
                                    // currentRoute. TotalToll
                                }
                                else if (reader.Name == "TotalToll")
                                {
                                    currentRoute.TotalToll = int.Parse(builder);
                                }
                                else if (reader.Name == "TotalTime")
                                {
                                    currentRoute.totalTime = int.Parse(builder);
                                }
                                else if (reader.Name == "TotalLength")
                                {
                                    currentRoute.totalLength = float.Parse(builder);
                                }
                                else if (reader.Name == "Section")
                                {
                                    currentRoute.details.Add(currentSection);
                                    currentSection = null;
                                }
                                else if (reader.Name == "Length")
                                {
                                    if (currentSubSection != null)
                                    {
                                        currentSubSection.length = float.Parse(builder);
                                    }
                                    else
                                    {
                                        currentSection.length = float.Parse(builder);
                                    }

                                }
                                else if (reader.Name == "SubSection")
                                {
                                    currentSection.subSections.Add(currentSubSection);
                                    currentSubSection = null;
                                }
                                else if (reader.Name == "Road")
                                {
                                    currentSubSection.road = builder;
                                }
                                else if (reader.Name == "Time")
                                {
                                    if (currentSubSection != null)
                                    {
                                        currentSubSection.time = int.Parse(builder);
                                    }
                                    else
                                    {
                                        currentSection.time = int.Parse(builder);
                                    }
                                }
                                else if (reader.Name == "Toll")
                                {
                                    Data_Toll toll = new Data_Toll(builder);
                                    if (currentSection.tolls.Count != 0)
                                    {
                                        if (toll.time == TimeSaleType.TSAll) toll.time = TimeSaleType.TSEtc;
                                        if (toll.day == DaySaleType.DSAll) toll.day = DaySaleType.DSEtc;
                                    }
                                    toll.tollid = currentSection.tolls.Count;
                                    currentSection.tolls.Add(toll);

                                    Boolean check1, check2;
                                    check1 = check2 = false;
                                    switch (toll.time)
                                    { // 現在時刻から料金選択
                                        case TimeSaleType.TSAll: // All Times
                                            check1 = true;
                                            break;
                                        case TimeSaleType.TSNoon: // 昼間
                                            if (gTime >= 9 && gTime < 17)
                                                check1 = true;
                                            break;
                                        case TimeSaleType.TSNight22_6: // 夜間
                                            if (gTime >= 22 && gTime < 6)
                                                check1 = true;
                                            break;
                                        case TimeSaleType.TSNight0_4: // 深夜 : 0-4
                                            if (gTime >= 0 && gTime < 4)
                                                check1 = true;
                                            break;
                                        case TimeSaleType.TSNight20_24: // 深夜 : 20-24, 4-6
                                            if (gTime >= 20 && gTime < 24 && gTime >= 4 && gTime < 6)
                                                check1 = true;
                                            break;
                                        case TimeSaleType.TSWork: // 出勤割引
                                            if ((gTime >= 6 && gTime < 9)
                                                    || (gTime >= 17 && gTime < 20))
                                                check1 = true;
                                            break;
                                        case TimeSaleType.TSPeek: // ピーク
                                            if ((gTime >= 6 && gTime < 11)
                                                    || (gTime >= 15 && gTime < 18))
                                                check1 = true;
                                            break;
                                        case TimeSaleType.TSOff: // オフ
                                            if ((gTime >= 11 && gTime < 15)
                                                    || (gTime >= 18 && gTime < 22))
                                                check1 = true;
                                            break;
                                    }

                                    switch (toll.day)
                                    {
                                        case DaySaleType.DSAll: // All Day
                                            check2 = true;
                                            break;
                                        case DaySaleType.DSWeekDay: // 平日
                                            if (gDay == 0)
                                                check2 = true;
                                            break;
                                        case DaySaleType.DSHoliday: // 休日 : 土曜、日曜
                                            if (gDay > 0)
                                                check2 = true;
                                            break;
                                        case DaySaleType.DSSunDay: // 日曜日
                                            if (gDay == 2)
                                                check2 = true;
                                            break;
                                    }

                                    if (check1 && check2)
                                        currentSection.selectedToll = currentSection.tolls.Count - 1;

                                  
                                }

                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (this.DRs.routes.Count > 0)
            {
                this.DRs.setSubRouteID();
                this.routesList.ItemsSource = this.DRs.routes;
                this.pnlResult.Visibility = Visibility.Visible;

                addHist(this.startIc + "→" + this.endIC);
                this.labICs.Text = this.startIc;
                this.labICe.Text = this.endIC;
                this.btnSort.Content = sortTypes[this.DRs.sortBy];
                this.sortList.SelectedIndex = this.DRs.sortBy;
                this.msg.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (DRs.inited == false)
                {
                    msg.Text = "サーバーに接続できませんでした";

                }
                else if (DRs.status == Status.NotEnd)
                {
                    msg.Text = "ICに複数の候補があります";
                }
                else if (DRs.status == Status.NotFound)
                {
                    msg.Text = "経路がありません";
                }
                else if (DRs.status == Status.Nothing)
                {
                    msg.Text = "データを取得できませんでした";
                }
                else 
                {
                    msg.Text = "経路がありません";
                }
            }


        }


        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

            //string target = "/MainPage.xaml";
            //this.NavigationService.Navigate(new Uri(target, UriKind.Relative));
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            pnlResult.Visibility = Visibility.Collapsed;
            sortW.Visibility = Visibility.Visible;
        }

        public void addHist(string rt)
        {

            //get list string
            string rts = "";

            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //Create a new StreamReader
            StreamReader fileReader = null;
            try
            {
                fileReader = new StreamReader(new IsolatedStorageFileStream("hist.txt", FileMode.Open, fileStorage));
                rts = fileReader.ReadLine();
                fileReader.Close();
            }
            catch
            {
            }



            //edit history list
            if (rts.Length == 0)
            {
                rts = rt;
            }
            else if (rts.IndexOf(rt) < 0)
            {
                rts = rt + ";" + rts;
            }
            else if (rts.IndexOf(rt) > 0)
            {
                rts = rt + ";" + rts.Replace(rt, "").Replace(";;", ";");
            }
            else if (rts.Split(';').Length > 8)
                rts = rts.Substring(0, rts.LastIndexOf(';'));


            //write history list
            IsolatedStorageFile fileStorage2 = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter fileWriter = new StreamWriter(new IsolatedStorageFileStream("hist.txt", FileMode.OpenOrCreate, fileStorage2));
            fileWriter.WriteLine(rts);
            fileWriter.Close();

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (this.sortList.SelectedIndex == -1) return;

            if (this.DRs.sortBy != this.sortList.SelectedIndex)
            {

                this.DRs.sortBy = this.sortList.SelectedIndex;
                this.DRs.sortRoutes();
                this.btnSort.Content = sortTypes[this.DRs.sortBy];

                this.routesList.ItemsSource = null;
                this.routesList.Items.Clear();
                this.DRs.setSubRouteID();
                this.routesList.ItemsSource = this.DRs.routes;
            }

            sortW.Visibility = Visibility.Collapsed;
            pnlResult.Visibility = Visibility.Visible;

            this.sortList.SelectedIndex = -1;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button tmp= (sender as Button);
            string str =tmp.Tag.ToString();
            string[] ids=str.Split(':'); 
            
       
                //todo
                this.DRs.rid = int.Parse(ids[0]);
                this.DRs.sid = int.Parse(ids[1]);

                string target = "/detail.xaml";
                this.NavigationService.Navigate(new Uri(target, UriKind.Relative));


        }

    }
}