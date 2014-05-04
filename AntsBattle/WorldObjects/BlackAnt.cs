using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntsBattle
{
    class BlackAnt : WorldObject
    {
        public BlackAnt(Point location) : base(location)
		{
        }

        public override Object GetObjectType()
        {
            return Object.UnknownAnt;
        }

        public override AntColour GetColourOrNone()
        {
            return AntColour.Black;
        }

        public class Local
        {
            public Direction Direction = Direction.None;
            public Func<Point, AIWorld, Direction> Function;
            private readonly Point _location;
            private readonly AIWorld _world;

            public Local(Func<Point, AIWorld, Direction> function, Point location, AIWorld world)
            {
                Function = function;
                _location = location;
                _world = world;
            }

            public void StartFunction()
            {
                try
                {
                    Direction = Function(_location, _world);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public override void Act(World world)
        {
            world.SetLocation(Location);
            world.SetTurn(-1);
            Thread.Sleep(10);
            world.SetTurn(0);
            var destination = new Point(0, 0);
            if (world.GetDirection() == Direction.Up)
                destination.Y += 1;
            if (world.GetDirection() == Direction.Down)
                destination.Y -= 1;
            if (world.GetDirection() == Direction.Left)
                destination.X -= 1;
            if (world.GetDirection() == Direction.Right)
                destination.X += 1;
            if (world.GetObject(Location.Add(destination), AntColour.Black) == Object.Food ||
                world.GetObject(Location.Add(destination), AntColour.Black) == Object.EnemyAnt ||
                world.GetObject(Location.Add(destination), AntColour.Black) == Object.YourAnt ||
                world.GetObject(Location.Add(destination), AntColour.Black) == Object.None)
                Destination = Location.Add(destination);
            if (world.GetObject(Destination, AntColour.Black) == Object.Food)
            {
                world.RemoveObject(world.Cells[Destination].First(x => x.GetObjectType() == Object.Food));
                world.BlackScore++;
            }
            if (world.GetObject(Location, AntColour.Black) == Object.Food)
            {
                world.RemoveObject(world.Cells[Location].First(x => x.GetObjectType() == Object.Food));
                world.BlackScore++;
            }
            world.SetDirection(Direction.None);
        }
    }
}
