using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LevelGen
{
    internal static class Util
    {
        public static List<Color> TerrainColors = new List<Color>()
        {
            Color.FromArgb(126, 64, 25),
            Color.FromArgb(129, 64, 25),
            Color.FromArgb(132, 64, 25),
            Color.FromArgb(126, 67, 25),
            Color.FromArgb(132, 67, 25),
            Color.FromArgb(129, 67, 25),
            Color.FromArgb(126, 70, 25),
            Color.FromArgb(129, 70, 25),
            Color.FromArgb(132, 70, 25),

        };

        static int min = 0;
        static int max = 8;

        public static List<Color> WaterColors = new List<Color>()
        {
            Color.FromArgb(0, 146, 179),
            Color.FromArgb(0, 149, 179),
            Color.FromArgb(0, 152, 179),
            Color.FromArgb(0, 146, 182),
            Color.FromArgb(0, 149, 182),
            Color.FromArgb(0, 152, 182),
            Color.FromArgb(0, 146, 185),
            Color.FromArgb(0, 149, 185),
            Color.FromArgb(0, 152, 185),
        };

        public static float Distance(Point a, Point b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public static void MapSmoothing(int[,] map, int width, int height, int[,] colormap, int[,] waterColors)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            int uncolored = 0;
            int uncoloredWater = 0;
            int oldUncolored = uncolored;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == 1)
                    {
                        colormap[x, y] = -1;
                        waterColors[x, y] = -2;
                        uncolored++;
                    }
                    else
                    {
                        colormap[x, y] = -2;
                        waterColors[x, y] = -1;
                        uncoloredWater++;
                    }
                }
            }

            Point p = new Point(rand.Next(0, width), rand.Next(0, height));

            while (colormap[p.X, p.Y] != -1)
            {
                p = new Point(rand.Next(0, width), rand.Next(0, height));
            }

            colormap[p.X, p.Y] = min / 2 + max / 2;

            int[,] dimensions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            while (uncolored > 1)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (colormap[x, y] == -1)
                        {
                            int sum = 0;
                            int count = 0;

                            for (int i = 0; i < 4; i++)
                            {
                                if (x + dimensions[i, 0] >= width)
                                    continue;

                                if (x + dimensions[i, 0] < 0)
                                    continue;

                                if (y + dimensions[i, 1] >= height)
                                    continue;

                                if (y + dimensions[i, 1] < 0)
                                    continue;

                                if (colormap[x + dimensions[i, 0], y + dimensions[i, 1]] > -1)
                                {
                                    count++;
                                    sum += colormap[x + dimensions[i, 0], y + dimensions[i, 1]];
                                }
                            }

                            if (count != 0)
                            {
                                int index = (sum / count) + rand.Next(-2, 3);

                                if (index > max) index = max;

                                if (index < min) index = min;

                                colormap[x, y] = index;

                                uncolored--;
                            }
                            else
                            {
                                colormap[x, y] = rand.Next(0, TerrainColors.Count);
                                uncolored--;
                            }
                        }
                    }
                }

                if (uncolored == oldUncolored)
                {
                    while (colormap[p.X, p.Y] != -1)
                    {
                        p = new Point(rand.Next(0, width), rand.Next(0, height));
                    }
                }
                oldUncolored = uncolored;
            }

            oldUncolored = uncoloredWater;

            p = new Point(rand.Next(0, width), rand.Next(0, height));

            while (waterColors[p.X, p.Y] != -1)
            {
                p = new Point(rand.Next(0, width), rand.Next(0, height));
            }

            waterColors[p.X, p.Y] = min / 2 + max / 2;

            while (uncoloredWater > 1)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (waterColors[x, y] == -1)
                        {
                            int sum = 0;
                            int count = 0;

                            for (int i = 0; i < 4; i++)
                            {
                                if (x + dimensions[i, 0] >= width)
                                    continue;

                                if (x + dimensions[i, 0] < 0)
                                    continue;

                                if (y + dimensions[i, 1] >= height)
                                    continue;

                                if (y + dimensions[i, 1] < 0)
                                    continue;

                                if (waterColors[x + dimensions[i, 0], y + dimensions[i, 1]] > -1)
                                {
                                    count++;
                                    sum += waterColors[x + dimensions[i, 0], y + dimensions[i, 1]];
                                }
                            }

                            if (count != 0)
                            {
                                int index = (sum / count) + rand.Next(-2, 3);

                                if (index > max) index = max;

                                if (index < min) index = min;

                                waterColors[x, y] = index;

                                uncoloredWater--;
                            }
                            else
                            {
                                waterColors[x, y] = rand.Next(0, TerrainColors.Count);
                                uncoloredWater--;
                            }
                        }
                    }
                }

                if (uncoloredWater == oldUncolored)
                {
                    while (waterColors[p.X, p.Y] != -1)
                    {
                        p = new Point(rand.Next(0, width), rand.Next(0, height));
                    }
                }

                oldUncolored = uncoloredWater;
            }


        }
    }
}
