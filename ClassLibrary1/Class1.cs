using System;
using System.Drawing;
using AntsBattle;

namespace ClassLibrary1
{
    public class Class1 : IAntAI
    {
        readonly Random _r = new Random();
        private int a = 1;
        public Direction GetDirection(Point currentAntLocation, AIWorld world)
        {
            for (int j = 0; j < 1000000; j++)
            {
                a++;
            }
            return Direction.Right;
            var i = _r.Next()%4;
            switch (i)
            {
                case 0:
                    return Direction.Right;
                case 1:
                    return Direction.Up;
                case 2:
                    return Direction.Left;
                case 3:
                    break;
            }
            return Direction.Down;
            //if (world.GetObjectInCell(new Point(currentAntLocation.X, currentAntLocation.Y + a)) == AntsBattle.Object.Wall)
            //    a *= -1;
            //return a == 1 ? Direction.Up : Direction.Down;
        }

        public string PlayerName { get { return "Random"; } }

    }
}
