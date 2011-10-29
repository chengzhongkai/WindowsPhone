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

    public partial class MarqueeText : UserControl
    {
        public event RoutedEventHandler Click;

        private TextBlock tbmarquee;
        private Canvas canMain;

        MarqueeType _marqueeType = MarqueeType.Center;

        public MarqueeType MarqueeType
        {
            get { return _marqueeType; }
            set { _marqueeType = value; }
        }

        private double _marqueeTimeInSeconds = 10;
        public double MarqueeTimeInSeconds
        {
            get { return _marqueeTimeInSeconds; }
            set { _marqueeTimeInSeconds = value; }
        }

        public static readonly DependencyProperty MarqueeContentProperty =
           DependencyProperty.Register("MarqueeContent", typeof(string), typeof(MarqueeText), new PropertyMetadata("Marquee",
                                  new PropertyChangedCallback(
                                    OnTheMarqueeContentChanged)));

        public string MarqueeContent
        {
            get { return (string)GetValue(MarqueeContentProperty); }
            set { SetValue(MarqueeContentProperty, value); }
        }

        public static readonly DependencyProperty MarqueeHeightProperty =
       DependencyProperty.Register("MarqueeHeight", typeof(double), typeof(MarqueeText), new PropertyMetadata((double)10,
                              new PropertyChangedCallback(
                                OnTheMarqueeHeightChanged)));

        public double MarqueeHeight
        {
            get { return (double)GetValue(MarqueeHeightProperty); }
            set { SetValue(MarqueeHeightProperty, value); }
        }

        public static readonly DependencyProperty MarqueeWidthProperty =
       DependencyProperty.Register("MarqueeWidth", typeof(double), typeof(MarqueeText), new PropertyMetadata((double)100,
                              new PropertyChangedCallback(
                                OnTheMarqueeWidthChanged)));

        public double MarqueeWidth
        {
            get { return (double)GetValue(MarqueeWidthProperty); }
            set { SetValue(MarqueeWidthProperty, value); }
        }

        public static readonly DependencyProperty MarqueeBackgroundProperty =
     DependencyProperty.Register("MarqueeBackground", typeof(Brush), typeof(MarqueeText), new PropertyMetadata(new SolidColorBrush(Colors.Red),
                            new PropertyChangedCallback(
                              OnTheMarqueeBackgroundChanged)));

        public Brush MarqueeBackground
        {
            get { return (Brush)GetValue(MarqueeBackgroundProperty); }
            set { SetValue(MarqueeBackgroundProperty, value); }
        }

        public MarqueeText()
        {
            comm();
        }
        public MarqueeText(double width, double height)
        {
 comm();
            this.MarqueeHeight = height;
            this.MarqueeWidth = width;
           
        }
        public void comm()
        {
            //InitializeComponent();

            if (double.IsNaN(this.Height))
            {
                this.Height = 50;
            }

            if (double.IsNaN(this.Width))
            {
                this.Width = 200;
            }


            tbmarquee = new TextBlock();
            canMain = new Canvas();

            canMain.Height = this.Height;
            canMain.Width = this.Width;
            canMain.Children.Add(tbmarquee);

            base.Content = canMain;
            this.Loaded += new RoutedEventHandler(MarqueeText_Loaded);

            // this.DefaultStyleKey = typeof(MarqueeText);
            this.MouseLeftButtonUp += new MouseButtonEventHandler
                (MarqueeText_MouseLeftButtonUp);
        }

        void MarqueeText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click(this, new RoutedEventArgs());
        }


        static void OnTheMarqueeBackgroundChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((MarqueeText)sender).Background = (Brush)args.NewValue;
            ((MarqueeText)sender).canMain.Background = (Brush)args.NewValue;
        }

        static void OnTheMarqueeWidthChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((MarqueeText)sender).Width = Convert.ToDouble(args.NewValue);
            ((MarqueeText)sender).canMain.Width = Convert.ToDouble(args.NewValue);
        }

        static void OnTheMarqueeHeightChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((MarqueeText)sender).Height = Convert.ToDouble(args.NewValue);
            ((MarqueeText)sender).canMain.Height = Convert.ToDouble(args.NewValue);
        }
        static void OnTheMarqueeContentChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((MarqueeText)sender).tbmarquee.Text = Convert.ToString(args.NewValue);
            ((MarqueeText)sender).StartMarqueeing(((MarqueeText)sender)._marqueeType);
        }

        void MarqueeText_Loaded(object sender, RoutedEventArgs e)
        {
            StartMarqueeing(_marqueeType);
        }

        public void StartMarqueeing(MarqueeType marqueeType)
        {
            if (marqueeType == MarqueeType.LeftToRight)
            {
                LeftToRightMarquee();
            }
            else if (marqueeType == MarqueeType.RightToLeft)
            {
                RightToLeftMarquee();
            }
            else if (marqueeType == MarqueeType.TopToBottom)
            {
                TopToBottomMarquee();
            }
            else if (marqueeType == MarqueeType.BottomToTop)
            {
                BottomToTopMarquee();
            }
            else if (marqueeType == MarqueeType.Center)
            {
                Center();
            }
        }
        private void LeftToRightMarquee()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(canMain.Width, canMain.Height));
            canMain.Clip = rectangleGeometry;
            double height = canMain.Height - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.Width;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, tbmarquee);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();
        }
        private void RightToLeftMarquee()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(canMain.Width, canMain.Height));
            canMain.Clip = rectangleGeometry;
            double height = canMain.Height - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = canMain.Width;
            doubleAnimation.To = -tbmarquee.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, tbmarquee);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();

        }
        private void TopToBottomMarquee()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(canMain.Width, canMain.Height));
            canMain.Clip = rectangleGeometry;
            double width = canMain.Width - tbmarquee.ActualWidth;
            tbmarquee.Margin = new Thickness(width / 2, 0, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualHeight;
            doubleAnimation.To = canMain.Height;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, tbmarquee);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Begin();
        }
        private void BottomToTopMarquee()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(canMain.Width, canMain.Height));
            canMain.Clip = rectangleGeometry;
            double width = canMain.Width - tbmarquee.ActualWidth;
            tbmarquee.Margin = new Thickness(width / 2, 0, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = canMain.Height;
            doubleAnimation.To = -tbmarquee.ActualHeight;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, tbmarquee);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Begin();
        }
        private void Center()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(canMain.Width, canMain.Height));
            canMain.Clip = rectangleGeometry;
            double width = canMain.Width - tbmarquee.ActualWidth;
            double height = canMain.Height - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(width / 2, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
  
           // doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
           // doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0));
            Storyboard storyboard = new Storyboard();
           // storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, tbmarquee);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Begin();
        }
            
    }
    public enum MarqueeType
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
        Center
    }


}
