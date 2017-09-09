using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _41m60t.Assets
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultPage : Page
    {
        public ResultPage()
        {
            this.InitializeComponent();
        }
        int ring1 = 0, ring2 = 0, ring3 = 0, shots = 1;

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double v = RingCanvas.ActualWidth * (shots - ring1 - ring2 - ring3) / shots;
            Rectangle rect1 = new Rectangle
            {
                Width = RingCanvas.ActualWidth * ring1 / shots,
                Height = RingCanvas.Height,
                Fill = new SolidColorBrush(Color.FromArgb(255, 66, 255, 33))
            };
            Canvas.SetLeft(rect1, v);
            Canvas.SetTop(rect1, 0);
            RingCanvas.Children.Add(rect1);


            Rectangle rect2 = new Rectangle
            {
                Width = RingCanvas.ActualWidth * ring2 / shots,
                Height = RingCanvas.Height,
                Fill = new SolidColorBrush(Color.FromArgb(255, 51, 204, 255))
            };
            Canvas.SetLeft(rect2, v + rect1.ActualWidth);
            Canvas.SetTop(rect2, 0);
            RingCanvas.Children.Add(rect2);

            Rectangle rect3 = new Rectangle
            {
                Width = RingCanvas.ActualWidth * ring3 / shots,
                Height = RingCanvas.Height,
                Fill = new SolidColorBrush(Color.FromArgb(255, 255, 102, 102))
            };
            Canvas.SetLeft(rect3, v + rect1.ActualWidth +rect2.ActualWidth);
            Canvas.SetTop(rect3, 0);
            RingCanvas.Children.Add(rect3);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Result r = (Result)e.Parameter;
            r.ShotPoints.ForEach(point =>
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 5,
                    Height = 5,
                    Fill = new SolidColorBrush(Colors.Black)
                };
                double x = point.x * 150 / (2 * Target.initial_radius);
                double y = point.y * 150 / (2 * Target.initial_radius);
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
                ResultCanvas.Children.Add(ellipse);
            });
            TimeTextBlock.Text = (r.time / 1000.0).ToString("0.00");
            TotalShotsTextBlock.Text = "" +  r.shots;
            MissedTextBlock.Text = "" + r.missed;
            HitMissedTextBlock.Text = (100.0 * (r.shots - r.missed) / r.shots).ToString("0.00") + "%";
            r1h.Text = "" + r.ring1;
            r2h.Text = "" + r.ring2;
            r3h.Text = "" + r.ring3;

            r1p.Text = (100.0 * r.ring1 / r.shots).ToString("0.00") + "%";
            r2p.Text = (100.0 * r.ring2 / r.shots).ToString("0.00") + "%";
            r3p.Text = (100.0 * r.ring3 / r.shots).ToString("0.00") + "%";
            ring1 = r.ring1;
            ring2 = r.ring2;
            ring3 = r.ring3;
            shots = r.shots;
            if(r.shots == 0)
            {
                r1p.Text = "0.00%";
                r2p.Text = "0.00%";
                r3p.Text = "0.00%";
                HitMissedTextBlock.Text = "0.00%";
            }
        }
    }
}
