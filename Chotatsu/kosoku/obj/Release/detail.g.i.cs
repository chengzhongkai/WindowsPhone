﻿#pragma checksum "C:\Users\cheng\Desktop\AndroidApp\kosoku\detail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "21B7F621C3F16F57722E9612A10EFD6F"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.237
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace kosoku {
    
    
    public partial class Page1 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Button btnResult;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock textRoute;
        
        internal System.Windows.Controls.ListBox subroutelist;
        
        internal System.Windows.Controls.ListBox tollsList;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/kosoku;component/detail.xaml", System.UriKind.Relative));
            this.btnResult = ((System.Windows.Controls.Button)(this.FindName("btnResult")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.textRoute = ((System.Windows.Controls.TextBlock)(this.FindName("textRoute")));
            this.subroutelist = ((System.Windows.Controls.ListBox)(this.FindName("subroutelist")));
            this.tollsList = ((System.Windows.Controls.ListBox)(this.FindName("tollsList")));
        }
    }
}

