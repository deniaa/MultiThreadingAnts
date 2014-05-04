using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsBattle
{
    public enum Object
    {
        None, EnemyAnt, Wall, Frog, YourAnt, Food, UnknownAnt
    }

    public class AIWorld
    {
        private readonly AntColour _colour;

        public AIWorld(AntColour colour)
        {
            _colour = colour;
        }

        public Object GetObjectInCell(Point location)
        {
            Console.WriteLine("GetObject " + location.X + ";" + location.Y + " " + _colour);
            var answer = Console.ReadLine();
            return ConvertObjectInEnum(answer);
        }

        private Object ConvertObjectInEnum(string answer)
        {
            switch (answer)
            {
                case "UnknownAnt":
                    return Object.UnknownAnt;
                case "EnemyAnt":
                    return Object.EnemyAnt;
                case "YourAnt":
                    return Object.YourAnt;
                case "Wall":
                    return Object.Wall;
                case "Food":
                    return Object.Food;
                case "Frog":
                    return Object.Frog;
                default:
                    return Object.None;
            }
        }

        public Size MapSize { get { return GetWorldSize(); } }

        private Size GetWorldSize()
        {
            Console.WriteLine("Size");
            var size = (Console.ReadLine()??"0 0").Split(' ').Select(int.Parse).ToList();
            return new Size {Width = size[0], Height = size[1]};
        }

        public int FrogSleepTime { get { return GetFrogSleepTime(); } }

        private int GetFrogSleepTime()
        {
            Console.WriteLine("SleepTime");
            return int.Parse(Console.ReadLine() ?? "0");
        }

        public int FrogMouthLength { get { return GetMouthLength(); } }

        private int GetMouthLength()
        {
            Console.WriteLine("MouthLength");
            return int.Parse(Console.ReadLine() ?? "0");
        }
    }
}
