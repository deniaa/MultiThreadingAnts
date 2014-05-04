using System;
using System.Drawing;

namespace AntsBattle
{
    public static class PointExtensions
    {
        public static Size Scale(this Size size, int factor)
        {
            return new Size(size.Width * factor, size.Height * factor);
        }

        public static Point Add(this Point a, Point b)
        {
            return a + (Size)b;
        }

        public static Point Mult(this Point a, int factor)
        {
            return new Point(a.X * factor, a.Y * factor);
        }

        public static Point Sub(this Point a, Point b)
        {
            return a - (Size)b;
        }

        public static int ManhattanLength(this Point a)
        {
            return Math.Abs(a.X) + Math.Abs(a.Y);
        }
    }
}