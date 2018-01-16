using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace _41m60t
{

    public class Target
    {
        public static int initial_radius = 35;
        public float radius;
        private int life;
        private int age = 0;
        private bool hited = false;
        private Canvas _main_canvas;
        public delegate void DieHandler(Target t);
        public event DieHandler DieEvent;
        public delegate void HitHandler(Target t,double x, double y);
        public event HitHandler HitEvent;
        public bool Hited
        {
            get
            {
                return hited;
            }
            set
            {
                hited = value;
                if (value == true)
                {
                    CircleShape1.Fill = new SolidColorBrush(Colors.Red);
                    CircleShape2.Fill = new SolidColorBrush(Colors.Red);
                    CircleShape3.Fill = new SolidColorBrush(Colors.Red);
                    died = true;
                }
            }
        }

        private Shape CircleShape1, CircleShape2, CircleShape3;
        private int x, y;
        private void _SetRadius(float radius)
        {
            this.radius = radius;
            CircleShape1.Width = radius * 2;
            CircleShape1.Height = radius * 2;
            CircleShape2.Width = radius * 4.0 / 3;
            CircleShape2.Height = radius * 4.0 / 3;
            CircleShape3.Width = radius * 2.0 / 3;
            CircleShape3.Height = radius * 2.0 / 3;
            Canvas.SetTop(CircleShape1, y - radius);
            Canvas.SetLeft(CircleShape1, x - radius);
            Canvas.SetTop(CircleShape2, y - radius * 2 / 3.0);
            Canvas.SetLeft(CircleShape2, x - radius * 2 / 3.0);
            Canvas.SetTop(CircleShape3, y - radius * 2 / 6.0);
            Canvas.SetLeft(CircleShape3, x - radius * 2 / 6.0);
        }
        public Target(Canvas canvas, int x, int y, int life = 50)
        {
            Color TargetColor = App.main_color;
            _main_canvas = canvas;
            this.x = x;
            this.y = y;
            this.life = life;
            CircleShape1 = new Ellipse()
            {
                Fill = new SolidColorBrush(TargetColor),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 1
            };
            CircleShape2 = new Ellipse()
            {
                Fill = new SolidColorBrush(TargetColor),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 1
            };
            CircleShape3 = new Ellipse()
            {
                Fill = new SolidColorBrush(TargetColor),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 1
            };
            Canvas.SetZIndex(CircleShape1, 10);
            Canvas.SetZIndex(CircleShape2, 10);
            Canvas.SetZIndex(CircleShape3, 10);
            _SetRadius(0);
            canvas.Children.Add(CircleShape1);
            canvas.Children.Add(CircleShape2);
            canvas.Children.Add(CircleShape3);
            CircleShape1.PointerPressed += Target_PointerPressed1;
            CircleShape2.PointerPressed += Target_PointerPressed2;
            CircleShape3.PointerPressed += Target_PointerPressed3;
        }
        int ring = 0;
        public int Ring { get { return ring; }  }
        public void Step()
        {
            if (age++ < life)
            {
                float anim = 1 - 2 * (float)Math.Abs((float)age / life - 0.5);
                if (!hited)
                    _SetRadius(anim * initial_radius);
                CircleShape1.Opacity = anim;
                CircleShape2.Opacity = anim;
                CircleShape3.Opacity = anim;
            }
            else
            {
                if (!died)
                {
                    _main_canvas.Children.Remove(CircleShape1);
                    _main_canvas.Children.Remove(CircleShape2);
                    _main_canvas.Children.Remove(CircleShape3);
                    DieEvent?.Invoke(this);
                    died = true;
                    visiable = false;
                }

            }
        }
        private bool died = false;
        private bool visiable = true;
        public bool Visable
        {
            get
            {
                return visiable;
            }
        }
        private void Target_PointerPressed1(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p =e.GetCurrentPoint(CircleShape1).Position;
            ring = 1;
            HitEvent?.Invoke(this, p.X, p.Y);
            Hited = true;
            Canvas.SetZIndex(CircleShape1, 1);
            Canvas.SetZIndex(CircleShape2, 1);
            Canvas.SetZIndex(CircleShape3, 1);
        }
        private void Target_PointerPressed2(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(CircleShape1).Position;
            ring = 2;
            HitEvent?.Invoke(this, p.X, p.Y);
            Hited = true;
            Canvas.SetZIndex(CircleShape1, 1);
            Canvas.SetZIndex(CircleShape2, 1);
            Canvas.SetZIndex(CircleShape3, 1);
        }
        private void Target_PointerPressed3(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(CircleShape1).Position;
            ring = 3;
            HitEvent?.Invoke(this, p.X, p.Y);
            Hited = true;
            Canvas.SetZIndex(CircleShape1, 1);
            Canvas.SetZIndex(CircleShape2, 1);
            Canvas.SetZIndex(CircleShape3, 1);
        }
    }
}
