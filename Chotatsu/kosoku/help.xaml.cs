using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
namespace kosoku
{
    public partial class help : PhoneApplicationPage
    {
        public help()
        {
            InitializeComponent();
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
            //string target = "/MainPage.xaml";
            //this.NavigationService.Navigate(new Uri(target, UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // タスクを新しく作成する
            var task = new WebBrowserTask();

            // Microsoft のページを表示したい
           // task.URL = "http://kosoku.jp/k/";
            task.Uri = new Uri("http://kosoku.jp/k/", UriKind.Absolute);

            // タスクを実行する
            task.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // タスクを新しく作成する
            var task = new WebBrowserTask();

            // Microsoft のページを表示したい
           // task.URL = "http://www.goga.co.jp";
            task.Uri = new Uri("http://www.goga.co.jp", UriKind.Absolute);

            // タスクを実行する
            task.Show();
        }

    }
}