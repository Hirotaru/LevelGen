using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LevelGen
{
    using static Math;
    public class Landscape
    {
        public static int[,] MapGeneration(int width, int height)
        {
            int[,] map = new int[width, height];
            Random rand = new Random();
            int seed = rand.Next();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = Util.waterDefaultValue;
                }
            }

            List<Point> seeds = new List<Point>();

            int arg = width * height / 85;

            for (int i = 0; i < Ceiling(Sin(arg) + Cos(arg) + arg / 5.5) + 5; i++)
            {
                Point t = new Point(rand.Next(0, width), rand.Next(0, height));
                seeds.Add(t);
                map[seeds[i].X, seeds[i].Y] = Util.groundDefaultValue;
            }

            int b = (int)(Sqrt(arg) + arg / 15) * 100;

            for (int i = 0; i < b; i++)
            {
                for (int s = 0; s < seeds.Count; s++)
                {

                    int dir = rand.Next(0, 4);
                    int val = Util.groundDefaultValue;

                    switch (dir)
                    {
                        case 0: //Move Up
                            {
                                Point p = seeds[s];
                                p.Y--;
                                if (p.Y >= 0)
                                {
                                    seeds[s] = p;
                                    map[seeds[s].X, seeds[s].Y] = val;
                                }
                                break;
                            }

                        case 1: //Move Down
                            {
                                Point p = seeds[s];
                                p.Y++;
                                if (p.Y < height)
                                {
                                    seeds[s] = p;

                                    map[seeds[s].X, seeds[s].Y] = val;
                                }

                                break;
                            }

                        case 2: //Move Left
                            {
                                Point p = seeds[s];
                                p.X--;
                                if (p.X >= 0)
                                {
                                    seeds[s] = p;

                                    map[seeds[s].X, seeds[s].Y] = val;
                                }
                                break;
                            }

                        case 3: //Move Right
                            {
                                Point p = seeds[s];
                                p.X++;
                                if (p.X < width)
                                {
                                    seeds[s] = p;

                                    map[seeds[s].X, seeds[s].Y] = val;
                                }
                                break;
                            }
                    }
                }
            }

            ImproveMap(map, width, height);

            GenerateWalls(map, width, height);

            return map;
        }

        private static void ImproveMap(int[,] map, int width, int height)
        {
            int[,] offsets = new int[8, 2]
            { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 },
            { 1, 1 },{ 0, 1 },{ -1, 1 },{ -1, 0 }};


            for (int i = 0; i < 25; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (map[x, y] == Util.waterDefaultValue)
                        {
                            int c = 0;

                            for (int k = 0; k < 8; k++)
                            {
                                if ((x + offsets[k, 0] < width && x + offsets[k, 0] >= 0) &&
                                    (y + offsets[k, 1] < height && y + offsets[k, 1] >= 0))
                                {
                                    if (map[x + offsets[k, 0], y + offsets[k, 1]] == Util.waterDefaultValue) c++;
                                }
                            }

                            if (c < 4)
                            {
                                map[x, y] = Util.groundDefaultValue;
                            }

                        }
                        else
                        {
                            int c = 0;

                            for (int k = 0; k < 8; k++)
                            {
                                if ((x + offsets[k, 0] < width && x + offsets[k, 0] >= 0) &&
                                    (y + offsets[k, 1] < height && y + offsets[k, 1] >= 0))
                                {
                                    if (map[x + offsets[k, 0], y + offsets[k, 1]] == 1) c++;
                                }
                            }

                            if (c < 4)
                            {
                                map[x, y] = Util.waterDefaultValue;
                            }
                        }
                    }
                }
            }

            if (map[0, 1] == Util.groundDefaultValue)
            {
                map[0, 0] = Util.groundDefaultValue;
            }
            else
            {
                map[0, 0] = Util.waterDefaultValue;
            }

            if (map[0, height - 2] == Util.groundDefaultValue)
            {
                map[0, height - 1] = Util.groundDefaultValue;
            }
            else
            {
                map[0, height - 1] = Util.waterDefaultValue;
            }

            if (map[width - 2, 0] == Util.groundDefaultValue)
            {
                map[width - 1, 0] = Util.groundDefaultValue;
            }
            else
            {
                map[width - 1, 0] = Util.waterDefaultValue;
            }

            if (map[width - 2, height - 2] == Util.groundDefaultValue)
            {
                map[width - 1, height - 1] = Util.groundDefaultValue;
            }
            else
            {
                map[width - 1, height - 1] = Util.waterDefaultValue;
            }
        }

        private static void GenerateWalls(int[,] map, int width, int height)
        {
            int[,] offsets = new int[8, 2]
            { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 },
            { 1, 1 },{ 0, 1 },{ -1, 1 },{ -1, 0 }};

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == Util.groundDefaultValue)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((x + offsets[i, 0] >= width) || (x + offsets[i, 0] < 0) || (y + offsets[i, 1] >= height) || (y + offsets[i, 1] < 0))
                            {
                                continue;
                            }
                            if (map[x + offsets[i, 0], y + offsets[i, 1]] == Util.waterDefaultValue)
                            {
                                map[x, y] = Util.shoreDefaultValue;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
