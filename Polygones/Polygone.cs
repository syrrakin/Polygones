using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Polygones
{
    internal class Polygone
    {
        public List<Point> Points { get; } = new List<Point>();

        public override string ToString()
        {
            return string.Join(" ", Points.Select(p => p.ToString()));
        }
    }
}
