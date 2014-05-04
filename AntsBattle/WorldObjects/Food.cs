using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsBattle
{
    class Food : WorldObject
    {
        public Food(Point location) : base(location)
		{
        }

        public override Object GetObjectType()
        {
            return Object.Food;
        }
    }
}
