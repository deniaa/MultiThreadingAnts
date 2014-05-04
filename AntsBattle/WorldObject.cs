using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsBattle
{
    public class WorldObject
    {
        public WorldObject(Point location)
        {
            Location = location;
            Destination = location;
        }

        public Point Location;
        public Point Destination;

        public virtual void Act(World world)
        {
        }

        public virtual AntColour GetColourOrNone()
        {
            return AntColour.None;
        }

        public virtual Object GetObjectType()
        {
            return Object.None;
        }

        public virtual Image GetImage(Images images, long time)
        {
            return images.GetImage(GetType().Name);
        }

    }
}
