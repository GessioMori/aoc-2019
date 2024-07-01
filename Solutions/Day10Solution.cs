using Main.Tools;
using System.Collections.Generic;

namespace Main.Solutions
{
    public class Day10Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            int[][] grid = ParseGrid(inputData);

            Dictionary<(int, int), List<((int, int), double)>> asteroids = GetAsteroids(grid);

            return asteroids.Values
                .Select(l => l.Select(i => i.Item2).Distinct().ToList())
                .ToList()
                .Max(l => l.Count)
                .ToString();
        }

        public string RunPartB(string[] inputData)
        {
            int[][] grid = ParseGrid(inputData);

            Dictionary<(int, int), List<((int, int), double)>> asteroids = GetAsteroids(grid);

            (int, int) bestPosition = asteroids
                .OrderByDescending(kvp => kvp.Value.Select(item => item.Item2).Distinct().Count())
                .First()
                .Key;

            List<((int, int), double)> otherAsteroids = asteroids[bestPosition]
                .OrderByDescending(a => a.Item2)
                .ThenBy(a => a.Item1.Distance(bestPosition))
                .ToList();

            List<double> usedAngles = [];
            List<(int, int)> clearedAsteroids = [];

            int counter = 0;

            while (counter < 200)
            {
                foreach (((int, int), double) asteroid in otherAsteroids)
                {
                    if (!usedAngles.Contains(asteroid.Item2) && !clearedAsteroids.Contains(asteroid.Item1))
                    {
                        clearedAsteroids.Add(asteroid.Item1);
                        usedAngles.Add(asteroid.Item2);
                        counter++;
                    }
                }

                usedAngles.Clear();
            }

            int result = clearedAsteroids[199].Item1 * 100 + clearedAsteroids[199].Item2;


            return result.ToString();
        }

        private static int[][] ParseGrid(string[] inputData)
        {
            int[][] grid = new int[inputData[0].Length][];

            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new int[inputData.Length];
            }

            for (int i = 0; i < inputData.Length; i++)
            {
                string row = inputData[i];

                for (int j = 0; j < row.Length; j++)
                {
                    grid[j][i] = row[j] == '#' ? 1 : 0;
                }
            }

            return grid;
        }

        private double CalculateAngle((int, int) ast1, (int, int) ast2)
        {
            double deltaX = ast2.Item1 - ast1.Item1;
            double deltaY = ast2.Item2 - ast1.Item2;

            return Math.Atan2(deltaX, deltaY);
        }

        private Dictionary<(int, int), List<((int, int), double)>> GetAsteroids(int[][] grid)
        {
            Dictionary<(int, int), List<((int, int), double)>> asteroids = [];

            for (int i = 0; i < grid[0].Length; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                {
                    if (grid[j][i] == 1)
                    {
                        asteroids.Add((j, i), []);
                    }
                }
            }

            foreach ((int, int) asteroid in asteroids.Keys)
            {
                foreach ((int, int) target in asteroids.Keys)
                {
                    if (!asteroid.Equals(target))
                    {
                        double angle = CalculateAngle(asteroid, target);
                        asteroids[asteroid].Add((target, angle));
                    }
                }
            }

            return asteroids;
        }
    }
}