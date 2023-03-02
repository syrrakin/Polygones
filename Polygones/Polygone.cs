using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Polygones
{
    internal class Polygone
    {
        public List<Point> Points { get; }

        public Polygone(params Point[] points)
        {
            Points = new List<Point>(points);
        }
        public override string ToString()
        {
            return string.Join(" ", Points.Select(p => p.ToString()));
        }
    }
}
