using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGen
{
    public partial class Map
    {
        static private int width;
        static private int height;

        static List<Room> rooms;

        static int mapDefaultValue = 0;

        public Map(int width, int height)
        {
            Map.width = width;
            Map.height = height;
        }


    }
}
