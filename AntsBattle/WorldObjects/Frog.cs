using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsBattle
{
    class Frog : WorldObject
    {
        public Frog(Point location) : base(location)
		{
        }

        private int _sleepTime;

        public override Object GetObjectType()
        {
            return Object.Frog;
        }

        public override void Act(World world)
        {
            if (_sleepTime >= 0)
            {
                var ants = new List<Point>();
                for (var i = Location.Y - world.FrogMouthLength; i <= Location.Y + world.FrogMouthLength; ++i)
                {
                    for (var j = Location.X - world.FrogMouthLength; j <= Location.X + world.FrogMouthLength; ++j)
                    {
                        if (world.GetObject(new Point(j, i), AntColour.None) == Object.UnknownAnt)
                            ants.Add(new Point(j, i));
                    }
                }
                if (ants.Count == 0)
                    return;
                var rand = new Random();
                var frogTarget = ants[rand.Next(ants.Count)];
                world.RemoveObject(world.Cells[frogTarget].First());
                _sleepTime = -world.FrogWantToSleep;
            }
            else
            {
                _sleepTime++;
            }
        }
    }
}
