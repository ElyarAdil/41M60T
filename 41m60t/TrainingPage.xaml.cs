using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
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
    public sealed partial class TrainingPage : Page
    {
        ToggleSwitch SoundToggleSwitch;
        ToggleSwitch InvToggleSwitch;
        TextBlock Life1TextBlock;
        TextBlock Life2TextBlock;
        TextBlock Life3TextBlock;
        TextBlock Life4TextBlock;
        TextBlock Life5TextBlock;
        TextBlock ShotsTextBlock;
        TextBlock MissedTextBlock;
        TextBlock PercentageTextBlock;

        TextBlock Ring1TextBlock;
        TextBlock Ring2TextBlock;
        TextBlock Ring3TextBlock;
        TextBlock Rp1TextBlock;
        TextBlock Rp2TextBlock;
        TextBlock Rp3TextBlock;

        TextBlock SpeedTextBlock;

        public TrainingPage()
        {
            this.InitializeComponent();


        }
        int shots = 0;
        List<Point> ShotPoints;
        int missed = 0;
        int msec_passed = 0;
        private bool running = false;
        Windows.Storage.StorageFile MissSoundFile;
        Windows.Storage.StorageFile ShotSoundFile;
        Windows.Storage.StorageFile ErrorSoundFile;
        private IRandomAccessStream MissStream;
        private IRandomAccessStream ShotStream;
        private IRandomAccessStream ErrorStream;
        TextBlock TimeTextBlock;
        private List<Target> Targets = new List<Target>();
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SoundToggleSwitch = (ToggleSwitch)Frame.FindName("SoundToggle");
            InvToggleSwitch = (ToggleSwitch)Frame.FindName("InvincibleToggle");
            generate_speed = 2;
            ShotPoints = new List<Point>();
            msec_passed = 0;
            ring1 = 0;
            ring2 = 0;
            ring3 = 0;
            Ring1TextBlock = (TextBlock)Frame.FindName("Ring1TextBlock");
            Ring2TextBlock = (TextBlock)Frame.FindName("Ring2TextBlock");
            Ring3TextBlock = (TextBlock)Frame.FindName("Ring3TextBlock");
            Ring1TextBlock.Text = "-";
            Ring2TextBlock.Text = "-";
            Ring3TextBlock.Text = "-";
            SpeedTextBlock = (TextBlock)Frame.FindName("SpeedTextBlock");
            Rp1TextBlock = (TextBlock)Frame.FindName("Rp1TextBlock");
            Rp2TextBlock = (TextBlock)Frame.FindName("Rp2TextBlock");
            Rp3TextBlock = (TextBlock)Frame.FindName("Rp3TextBlock");
            Rp1TextBlock.Text = "-";
            Rp2TextBlock.Text = "-";
            Rp3TextBlock.Text = "-";

            shots = 0;
            missed = 0;
            TimeTextBlock = (TextBlock)Frame.FindName("TimeLeftTextBlock");
            PercentageTextBlock = (TextBlock)Frame.FindName("PercentageTextBlock");
            PercentageTextBlock.Text = "0%";
            TimeTextBlock.Text = "00.00";
            ((Button)Frame.FindName("StartButton")).Click += StartButton_Click ;

            Windows.Storage.StorageFolder AssestsFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            MissSoundFile = await AssestsFolder.GetFileAsync("miss.wav");
            ShotSoundFile = await AssestsFolder.GetFileAsync("gunshot.wav");
            ErrorSoundFile = await AssestsFolder.GetFileAsync("error.wav");
            MissStream = await MissSoundFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
            ShotStream = await ShotSoundFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
            ErrorStream = await ErrorSoundFile.OpenAsync(Windows.Storage.FileAccessMode.Read);

            Life1TextBlock = ((TextBlock)Frame.FindName("Life1TextBlock"));
            Life2TextBlock = ((TextBlock)Frame.FindName("Life2TextBlock"));
            Life3TextBlock = ((TextBlock)Frame.FindName("Life3TextBlock"));
            Life4TextBlock = ((TextBlock)Frame.FindName("Life4TextBlock"));
            Life5TextBlock = ((TextBlock)Frame.FindName("Life5TextBlock"));

            SetLife(5);

            ShotsTextBlock = (TextBlock)Frame.FindName("ShotsTextBlock");
            MissedTextBlock = (TextBlock)Frame.FindName("MissedTextBlock");
            ShotsTextBlock.Text = "0";
            MissedTextBlock.Text = "0";
            if (running)
            {
                running = false;
                return;
            }
            Random r = new Random();
            running = true;
            int elipse_time_msec = 50;
            for (int i = 0; running; i++)
            {
                if(i * elipse_time_msec % 1000 == 0)
                {
                    generate_speed += 0.01;
                    SpeedTextBlock.Text = "" + generate_speed.ToString("0.00") + " / s";
                }
            
                await Task.Delay(elipse_time_msec);
                msec_passed = i * elipse_time_msec;
                int sec_passed = msec_passed / 1000;
                if ((i) % (int)(1000.0 / (generate_speed) / elipse_time_msec) == 0)
                {
                    Target t = new Target(MainCanvas, r.Next((int)MainCanvas.ActualWidth), r.Next((int)MainCanvas.ActualHeight));
                    t.DieEvent += TargetDie;
                    t.HitEvent += TargetHit;
                    Targets.Add(t);
                }
                Targets.ForEach(p => p.Step());
                TimeTextBlock.Text = (sec_passed).ToString("00") + "." + (msec_passed / 10 % 100).ToString("00");
                for (int j = Targets.Count - 1; j >= 0; j--)
                {
                    if (!Targets[j].Visable)
                        Targets.RemoveAt(j);
                }
                for (int j = Holes.Count - 1; j >= 0; j--)
                {
                    Shape hole = Holes[j];
                    hole.Opacity -= 0.02f;
                    if (hole.Opacity <= 0)
                    {
                        Holes.RemoveAt(j);
                        MainCanvas.Children.Remove(hole);
                    }
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            running = (bool)e.Parameter;
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            running = false;
        }

        private void TargetHit(Target t,double x, double y)
        {
            ShotPoints.Add(new Point(x - t.radius, y - t.radius));
            switch (t.Ring)
            {
                case 1:
                    ring1++;
                    break;
                case 2:
                    ring2++;
                    break;
                case 3:
                    ring3++;
                    break;
            }
            Ring1TextBlock.Text = ring1.ToString();
            Ring2TextBlock.Text = ring2.ToString();
            Ring3TextBlock.Text = ring3.ToString();
            Rp1TextBlock.Text = (100.0 * ring1 / shots).ToString("0.00") + "%";
            Rp2TextBlock.Text = (100.0 * ring2 / shots).ToString("0.00") + "%";
            Rp3TextBlock.Text = (100.0 * ring3 / shots).ToString("0.00") + "%";
        }
        private void TargetDie(Target t)
        {
            if (SoundToggleSwitch.IsOn) { 
                MediaElement MyMediaElement = new MediaElement();
                MyMediaElement.SetSource(ErrorStream, ErrorSoundFile.FileType);
                MyMediaElement.Play();
            }
            if(!InvToggleSwitch.IsOn)
                DecreaseLife();
            if (life == 0)
            {
                running = false;
                ((Button)Frame.FindName("StartButton")).Content = "\uE102";
                Result r = new Result();
                r.ring1 = ring1;
                r.ring2 = ring2;
                r.ring3 = ring3;
                r.shots = shots;
                r.time = msec_passed;
                r.missed = missed;
                r.ShotPoints = ShotPoints;
                Frame.Navigate(typeof(ResultPage), r);
            }
        }
        private List<Shape> Holes = new List<Shape>();
        private int life = 5;
        private void MainCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            shots++;
            var MyMediaElement = new MediaElement();
            Shape hole;
            if (e.OriginalSource.GetType() != typeof(Ellipse))
            {
                missed++;
                // SetLife(life--);
                if (SoundToggleSwitch.IsOn)
                    MyMediaElement.SetSource(MissStream, MissSoundFile.FileType);
                hole = new Ellipse() { Fill = new SolidColorBrush(Colors.Red) };

            }
            else
            {
                if (SoundToggleSwitch.IsOn)
                    MyMediaElement.SetSource(ShotStream, ShotSoundFile.FileType);
                hole = new Ellipse() { Fill = new SolidColorBrush(Colors.Black) };
            }
            if(SoundToggleSwitch.IsOn)
                MyMediaElement.Play();


            double x = e.GetCurrentPoint(MainCanvas).Position.X;
            double y = e.GetCurrentPoint(MainCanvas).Position.Y;
            Canvas.SetZIndex(hole, 10);
            hole.Width = 6;
            hole.Height = 6;
            Canvas.SetLeft(hole, x - 3);
            Canvas.SetTop(hole, y - 3);
            MainCanvas.Children.Add(hole);
            Holes.Add(hole);

            ShotsTextBlock.Text = "" + shots;
            MissedTextBlock.Text = "" + missed;
            double percent = 100.0 * (shots - missed) / shots;
            if (percent < 85)
                PercentageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            else
                PercentageTextBlock.Foreground = new SolidColorBrush(Colors.White);
            PercentageTextBlock.Text = (percent).ToString("0.00") + "%";
            Rp1TextBlock.Text = (100.0 * ring1 / shots).ToString("0.00") + "%";
            Rp2TextBlock.Text = (100.0 * ring2 / shots).ToString("0.00") + "%";
            Rp3TextBlock.Text = (100.0 * ring3 / shots).ToString("0.00") + "%";
        }
        int ring1 = 0;
        int ring2 = 0;
        int ring3 = 0;
        private void DecreaseLife()
        {
            SetLife(--life);
        }
        private void SetLife(int l)
        {
            life = l;
            String heart_filled = "\uE0A5";
            String heart_unfilled = "\uE006";
            Life1TextBlock.Text = l >= 1 ? heart_filled : heart_unfilled;
            Life2TextBlock.Text = l >= 2 ? heart_filled : heart_unfilled;
            Life3TextBlock.Text = l >= 3 ? heart_filled : heart_unfilled;
            Life4TextBlock.Text = l >= 4 ? heart_filled : heart_unfilled;
            Life5TextBlock.Text = l >= 5 ? heart_filled : heart_unfilled;
        }
        private void MainCanvas_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Cross, 1);
        }

        private void MainCanvas_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);

        }
        double generate_speed = 2;
        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            for (int i = MainCanvas.Children.Count - 1; i >= 0; i--)
            {
                if(MainCanvas.Children[i] is Line)
                {
                    MainCanvas.Children.RemoveAt(i);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                var line = new Line();
                line.StrokeDashArray = new DoubleCollection();
                line.StrokeDashArray.Add(1);
                line.StrokeDashArray.Add(1);
                line.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
                line.X1 = 0;
                line.X2 = MainCanvas.ActualWidth;
                line.Y1 = i * MainCanvas.ActualHeight / 5;
                line.Y2 = i * MainCanvas.ActualHeight / 5;
                MainCanvas.Children.Add(line);
            }
            for (int i = 0; i < 7; i++)
            {
                var line = new Line();
                line.StrokeDashArray = new DoubleCollection();
                line.StrokeDashArray.Add(1);
                line.StrokeDashArray.Add(1);
                line.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
                line.Y1 = 0;
                line.Y2 = MainCanvas.ActualHeight;
                line.X1 = i * MainCanvas.ActualWidth / 6;
                line.X2 = i * MainCanvas.ActualWidth / 6;
                MainCanvas.Children.Add(line);
            }


        }
    }
}
