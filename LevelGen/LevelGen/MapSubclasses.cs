using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGen
{
    public partial class Map
    {

        private class Room
        {
            int x, y, w, h;

            public int X
            {
                get { return x; }
            }

            public int Y
            {
                get { return y; }
            }

            public int W
            {
                get { return w; }
            }

            public int H
            {
                get { return h; }
            }

            public bool Intersect(Room r)
            {
                return !(
                    (r.x >= (x + w)) ||
                    (x >= (r.x + r.w)) ||
                    (r.y >= (y + h)) ||
                    (y >= (r.y + r.h)));
            }

            public Room(int x, int y, int w, int h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }
        }
    }
}
