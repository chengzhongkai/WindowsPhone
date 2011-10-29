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
using System.IO.IsolatedStorage;
using System.IO;

namespace kosoku
{
    public partial class Hist : PhoneApplicationPage
    {
        private string[] rlist = null;
        public Hist()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string textFile = "";

            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //Create a new StreamReader
            StreamReader fileReader = null;
            try
            {
                //Read the file from the specified location.
                fileReader = new StreamReader(new IsolatedStorageFileStream("hist.txt", FileMode.Open, fileStorage));
                //Read the contents of the file (the only line we created).
                textFile = fileReader.ReadLine();
                //Write the contents of the file to the TextBlock on the page.
                // viewText.Text = textFile;
                fileReader.Close();
                rlist = textFile.Split(';');
                this.histList.ItemsSource = rlist;

            }
            catch
            {

                //If they click the view button first, we need to handle the fact that the file hasn't been created yet.
                // viewText.Text = "Need to create directory and the file first.";


            }

            if (rlist == null)
            {
                this.listBord.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.listBord.Visibility = Visibility.Visible;
            }

        }

        private void histList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox me = (sender as ListBox);
            if (me.SelectedIndex != -1 && me.SelectedIndex < rlist.Count())
            {
                int i = me.SelectedIndex;
                string target = "/MainPage.xaml";
                string[] ic = rlist[i].Split('→');
                target += string.Format("?f={0}&t={1}", ic);
                this.NavigationService.Navigate(new Uri(target, UriKind.Relative));

                me.SelectedIndex = -1;
            }
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }


    }
}