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

        int[,] colormap;
        int[,] waterColors;

        public static int[,] MapGeneration(int width, int height)
        {
            int[,] map = new int[width, height];
            Random rand = new Random();
            int seed = rand.Next();

            List<Point> seeds = new List<Point>();

            int arg = width * height / 100;

            for (int i = 0; i < Ceiling(Sin(arg) + Cos(arg) + arg / 10) + 5; i++)
            {
                Point t = new Point(rand.Next(0, width), rand.Next(0, height));
                seeds.Add(t);
                map[seeds[i].X, seeds[i].Y] = 1;
            }

            int b = (int)(Sqrt(arg) + arg / 15) * 100;

            for (int i = 0; i < b; i++)
            {
                for (int s = 0; s < seeds.Count; s++)
                {

                    int dir = rand.Next(0, 4);
                    int val = 1;

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
                        if (map[x, y] == 0)
                        {
                            int c = 0;

                            for (int k = 0; k < 8; k++)
                            {
                                if ((x + offsets[k, 0] < width && x + offsets[k, 0] >= 0) &&
                                    (y + offsets[k, 1] < height && y + offsets[k, 1] >= 0))
                                {
                                    if (map[x + offsets[k, 0], y + offsets[k, 1]] == 0) c++;
                                }
                            }

                            if (c < 4)
                            {
                                map[x, y] = 1;
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
                                map[x, y] = 0;
                            }
                        }
                    }
                }
            }

            if (map[0, 1] == 1)
            {
                map[0, 0] = 1;
            }
            else
            {
                map[0, 0] = 0;
            }

            if (map[0, height - 2] == 1)
            {
                map[0, height - 1] = 1;
            }
            else
            {
                map[0, height - 1] = 0;
            }

            if (map[width - 2, 0] == 1)
            {
                map[width - 1, 0] = 1;
            }
            else
            {
                map[width - 1, 0] = 0;
            }

            if (map[width - 2, height - 2] == 1)
            {
                map[width - 1, height - 1] = 1;
            }
            else
            {
                map[width - 1, height - 1] = 0;
            }
        }

        private static void GenerateWalls(int[,] map, int width, int height)
        {
            int[,] offsets = new int[8, 2]
            { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 },
            { 1, 1 },{ 0, 1 },{ -1, 1 },{ -1, 0 }};

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (map[x, y] == 1)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (map[x + offsets[i, 0], y + offsets[i, 1]] == 0)
                            {
                                map[x, y] = 2;
                                break;
                            }
                        }
                    }
                }
            }
        }



    }
}
