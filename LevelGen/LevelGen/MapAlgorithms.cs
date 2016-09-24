using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LevelGen
{
    public partial class Map
    {
        public static int[,] Generate(int roomsCount, int width, int height)
        {
            Map.width = width;
            Map.height = height;

            GenerateRooms(roomsCount);

            int[,] map = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = Util.mapDefaultValue;
                }
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int x = rooms[i].X; x <= rooms[i].X + rooms[i].W; x++)
                {
                    for (int y = rooms[i].Y; y <= rooms[i].Y + rooms[i].H; y++)
                    {
                        map[x, y] = Util.groundDefaultValue;
                    }
                }
            }

            GeneratePassages(map);

            GenerateWalls(map);

            ClearWalls(map);

            return map;
        }
        private static void GenerateWalls(int[,] map)
        {
            int[,] offsets = new int[8, 2]
            { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 },
            { 1, 1 },{ 0, 1 },{ -1, 1 },{ -1, 0 }};

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (map[x, y] == Util.mapDefaultValue)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (map[x + offsets[i, 0], y + offsets[i, 1]] == 1)
                            {
                                map[x, y] = 2;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static void GenerateRooms(int roomsCount)
        {
            int minRoomSize = Math.Min(width, height) / 30;
            int maxRoomSize = Math.Min(width, height) / 6;

            Random rand = new Random(DateTime.Now.Millisecond);
            rooms = new List<Room>();

            for (int i = 0; i < roomsCount; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    int w = rand.Next(minRoomSize, maxRoomSize);
                    int h = rand.Next(minRoomSize, maxRoomSize);

                    Room room = new Room(3 + rand.Next() % (width - w - 6), 3 + rand.Next() % (height - h - 6), w, h);
                    Room intersect = rooms.Find((r) => { return r.Intersect(room); });

                    if (intersect == null)
                    {
                        rooms.Add(room);
                    }
                }
            }
        }

        private static void ClearWalls(int[,] map)
        {
            int[,] offsets = new int[8, 2]
            { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 },
            { 1, 1 },{ 0, 1 },{ -1, 1 },{ -1, 0 }};

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (map[x, y] == 2)
                    {
                        int c = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            if (map[x + offsets[i, 0], y + offsets[i, 1]] != Util.mapDefaultValue)
                            {
                                c++;

                            }
                        }
                        if (c == 8)
                        {
                            map[x, y] = 1;
                        }
                    }
                }
            }
        }

        private static void GeneratePassages(int[,] map)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].X > width / 2)
                {
                    if (rooms[i].Y > height / 2)
                    {
                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X - 1, rooms[i].Y + rand.Next(1, rooms[i].H));

                            int d = 0;

                            while (map[p.X - d, p.Y] == Util.mapDefaultValue || map[p.X - d, p.Y - 1] == Util.mapDefaultValue || map[p.X - d, p.Y + 1] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.X - d < 0)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 0) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X - v, p.Y + 1] = 1;
                                map[p.X - v, p.Y - 1] = 1;
                                map[p.X - v, p.Y] = 1;
                            }
                            break;
                        }

                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X + rand.Next(1, rooms[i].W), rooms[i].Y - 1);
                            int d = 0;
                            while (map[p.X, p.Y - d] == Util.mapDefaultValue || map[p.X - 1, p.Y - d] == Util.mapDefaultValue || map[p.X + 1, p.Y - d] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.Y - d < 0)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 1) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X, p.Y - v] = 1;
                                map[p.X + 1, p.Y - v] = 1;
                                map[p.X - 1, p.Y - v] = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X - 1, rooms[i].Y + rand.Next(1, rooms[i].H));

                            int d = 0;

                            while (map[p.X - d, p.Y] == Util.mapDefaultValue || map[p.X - d, p.Y - 1] == Util.mapDefaultValue || map[p.X - d, p.Y + 1] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.X - d < 0)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 0) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X - v, p.Y] = 1;
                                map[p.X - v, p.Y + 1] = 1;
                                map[p.X - v, p.Y - 1] = 1;
                            }
                            break;
                        }

                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X + rand.Next(1, rooms[i].W), rooms[i].Y + rooms[i].H + 1);
                            int d = 1;
                            while (map[p.X, p.Y + d] == Util.mapDefaultValue || map[p.X - 1, p.Y + d] == Util.mapDefaultValue || map[p.X + 1, p.Y + d] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.Y + d == height)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 1) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X, p.Y + v] = 1;
                                map[p.X + 1, p.Y + v] = 1;
                                map[p.X - 1, p.Y + v] = 1;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (rooms[i].Y > height / 2)
                    {
                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X + rooms[i].W + 1, rooms[i].Y + rand.Next(1, rooms[i].H));

                            int d = 0;

                            while (map[p.X + d, p.Y] == Util.mapDefaultValue || map[p.X + d, p.Y - 1] == Util.mapDefaultValue || map[p.X + d, p.Y + 1] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.X + d == width)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 0) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X + v, p.Y] = 1;
                                map[p.X + v, p.Y - 1] = 1;
                                map[p.X + v, p.Y + 1] = 1;
                            }
                            break;
                        }

                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X + rand.Next(1, rooms[i].W), rooms[i].Y - 1);
                            int d = 0;
                            while (map[p.X, p.Y - d] == Util.mapDefaultValue || map[p.X - 1, p.Y - d] == Util.mapDefaultValue || map[p.X + 1, p.Y - d] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.Y - d < 0)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 1) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X, p.Y - v] = 1;
                                map[p.X + 1, p.Y - v] = 1;
                                map[p.X - 1, p.Y - v] = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X + rooms[i].W + 1, rooms[i].Y + rand.Next(1, rooms[i].H));

                            int d = 0;

                            while (map[p.X + d, p.Y] == Util.mapDefaultValue || map[p.X + d, p.Y - 1] == Util.mapDefaultValue || map[p.X + d, p.Y + 1] == Util.mapDefaultValue)
                            {

                                d++;

                                if (p.X + d == width)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 0) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X + v, p.Y] = 1;
                                map[p.X + v, p.Y + 1] = 1;
                                map[p.X + v, p.Y - 1] = 1;
                            }
                            break;
                        }

                        for (int k = 0; k < 1; k++)
                        {
                            Point p = new Point(rooms[i].X + rand.Next(1, rooms[i].W), rooms[i].Y + rooms[i].H + 1);
                            int d = 0;
                            while (map[p.X, p.Y + d] == Util.mapDefaultValue || map[p.X - 1, p.Y + d] == Util.mapDefaultValue || map[p.X + 1, p.Y + d] == Util.mapDefaultValue)
                            {
                                d++;

                                if (p.Y + d == height)
                                {
                                    d = 0;
                                    break;
                                }
                            }

                            if (d == 1) continue;

                            for (int v = 0; v <= d; v++)
                            {
                                map[p.X, p.Y + v] = 1;
                                map[p.X + 1, p.Y + v] = 1;
                                map[p.X - 1, p.Y + v] = 1;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
