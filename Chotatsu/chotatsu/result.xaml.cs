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

using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;

namespace chotatsu
{

    public partial class result : PhoneApplicationPage
    {

        Data_Result DRs = new Data_Result();
        int newpage = 1;
        string keyword = "";
        string org = "";
        string end = "";
        int sort = 0;
        string[] sortTypes = { "新着順", "期限順" };

        public result()
        {

            InitializeComponent();
            lsSort.ItemsSource = sortTypes;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {


            string value = string.Empty;
            IDictionary<string, string> queryString = this.NavigationContext.QueryString;
            queryString.TryGetValue("q", out value);
            if (value != null)
            {
                keyword = value;
                labKeyword.Text = keyword;
            }

            queryString.TryGetValue("org", out value);
            if (value != null)
            {
                org = value;
                labOrg.Text = org;
                pnlOrg.Visibility = Visibility.Visible;
            }
            else
            {
                pnlOrg.Visibility = Visibility.Collapsed;
            }

            queryString.TryGetValue("end", out value);
            if (value != null)
            {
                end = value;
                labDate.Text = end;
                pnlDate.Visibility = Visibility.Visible;
            }
            else
            {
                pnlDate.Visibility = Visibility.Collapsed;
            }


            getrss();



            base.OnNavigatedTo(e);
        }


        void getrss()

        {
            this.msg.Text = "データ取得中...";
            this.msg.Visibility = Visibility.Visible;
            WebClient cli = new WebClient();

            Uri uri;
            string url = "http://chotatsu.org/api.php?limit=50&q=" + keyword;

            if (org != "")
                url += "&org=" + org;
            if (end != "")
                url += "&end=" + end.Replace("年", "").Replace("月", "").Replace("日", "");

            url += "&page=" + newpage;

            url += "&s=" + sortTypes[sort];



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
                MessageBox.Show("エラー:" + e.Error.Message);
                return;
            }

            Stream stream = e.Result;
            this.DRs.details.Clear();
            try
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    // initialize variable



                    Data_Info currentItem = null;

                    string builder = "";

                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element: // ノードはエレメントです。


                                if (reader.Name == "Info")
                                {
                                    currentItem = new Data_Info();
                                }


                                break;
                            case XmlNodeType.Text: // 各エレメントのテキストを表示します。

                                builder = reader.Value;
                                break;
                            case XmlNodeType.EndElement: // エレメントの最後を表示します。


                                if (reader.Name == "Count")
                                {
                                    DRs.count = int.Parse(builder);
                                }
                                else if (reader.Name == "Keyword")
                                {
                                    DRs.keyword = builder;

                                }
                                else if (reader.Name == "Organization")
                                {
                                    if (currentItem != null)
                                    {
                                        currentItem.organization = builder;
                                    }
                                    else
                                    {
                                        DRs.organization = builder;
                                    }

                                }
                                else if (reader.Name == "Order")
                                {

                                    if (builder == "新着順")
                                    {
                                        DRs.order = 0;
                                    }
                                    else if (builder == "期限順")
                                    {
                                        DRs.order = 1;
                                    }

                                }
                                else if (reader.Name == "Limit")
                                {
                                    DRs.limit = int.Parse(builder);
                                }
                                else if (reader.Name == "Page")
                                {
                                    DRs.page = int.Parse(builder);
                                }
                                else if (reader.Name == "BidDeadline")
                                {
                                    currentItem.bidDeadline = DateTime.ParseExact(builder, "yy/MM/dd", null);

                                }
                                else if (reader.Name == "RegistryDate")
                                {
                                    currentItem.registryDate = DateTime.ParseExact(builder, "yy/MM/dd", null);
                                }
                                else if (reader.Name == "Subject")
                                {
                                    currentItem.subject = builder;
                                }
                                else if (reader.Name == "URL")
                                {
                                    currentItem.url = builder;
                                }
                                else if (reader.Name == "Info")
                                {
                                    DRs.details.Add(currentItem);
                                    currentItem = null;
                                }


                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }

            if (this.DRs.details.Count > 0)
            {
                this.infoList.ItemsSource = null;
                this.infoList.Items.Clear();
                this.infoList.ItemsSource = this.DRs.details;
                this.pnlResult.Visibility = Visibility.Visible;
                DRs.isNew = false;

                btnSort.Content = sortTypes[sort];

                LabPage.Text = "検索結果：" + newpage;

                if (newpage == 1)
                {
                    prePage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    prePage.Visibility = Visibility.Visible;
                }

                if (newpage * 50 > DRs.count)
                {
                    nextPage.Visibility = Visibility.Collapsed;
                }
                else
                {


                    nextPage.Visibility = Visibility.Visible;

                    if (newpage * 50 + 50 < DRs.count)
                    {
                        labNextPage.Text = "後の50件を検索する";
                    }
                    else
                    {
                        labNextPage.Text = "後の" + (DRs.count - newpage * 50) + "件を検索する";
                    }

                }

                msg.Visibility = Visibility.Collapsed;
                pnlResult.Visibility = Visibility.Visible;
                scrV.ScrollToVerticalOffset(0);
                btnSort.Visibility = Visibility.Visible;

            }
            else {
                msg.Text = "サーバーに接続できませんでした";
            }


        }



        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // タスクを新しく作成する
            var task = new WebBrowserTask();

            // Microsoft のページを表示したい
            //task.URL = (sender as Button).Tag.ToString();
            task.Uri = new Uri((sender as Button).Tag.ToString(), UriKind.Absolute);

            // タスクを実行する
            task.Show();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            newpage--;

            getrss();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            newpage++;
            getrss();
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (this.sortpop.Visibility == Visibility.Collapsed)
            {
                this.sortpop.Visibility = Visibility.Visible;
            }
            else { this.sortpop.Visibility = Visibility.Collapsed; }



            if (this.sortpop.Visibility == Visibility.Visible)
                this.pnlResult.Visibility = Visibility.Collapsed;
            else
                this.pnlResult.Visibility = Visibility.Visible;
        }

        private void setSort_Click(object sender, RoutedEventArgs e)
        {
            int cnt = lsSort.Items.Count;
            sortpop.Visibility = Visibility.Collapsed;
            if (sortTypes[sort].Equals((sender as Button).Content))
            {
                this.pnlResult.Visibility = Visibility.Visible;
            }
            else
            {
                if (sort == 0) { sort = 1; } else { sort = 0; }
                getrss();
            }


        }

    }
}