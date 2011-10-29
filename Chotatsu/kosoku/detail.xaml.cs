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

namespace kosoku
{
    public partial class Page1 : PhoneApplicationPage
    {
        Data_Result DRs = null;
        public Page1()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;
            if (app.DRs != null)
            {
                this.DRs = app.DRs;
            }

            int rid = this.DRs.rid;
            int sid = this.DRs.sid;
            this.textRoute.Text = DRs.routes[rid].details[sid].from+"→"+ DRs.routes[rid].details[sid].to;


            this.subroutelist.ItemsSource = this.DRs.routes[rid].details[sid].subSections;

            this.tollsList.ItemsSource = this.DRs.routes[rid].details[sid].tolls;

        }



        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int rid = this.DRs.rid;
            int sid = this.DRs.sid;

            int sel = int.Parse((sender as Button).Tag.ToString());

            this.DRs.routes[rid].details[sid].selectedToll = sel;
            this.DRs.isNew = true;
            this.NavigationService.GoBack();
        }
    }
}