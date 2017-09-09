using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _41m60t
{
    class Result
    {
        public int ring1 { get; set; }
        public int ring2 { get; set; }
        public int ring3 { get; set; }
        public int shots { get; set; }
        public int missed { get; set; }
        public int time { get; set; }
        public List<Point> ShotPoints  { get; set; }
    }
    class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
