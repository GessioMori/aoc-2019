using aoc_2019.Utils;
using Main.Tools;

namespace Main.Solutions
{
    public class Day11Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            Dictionary<(double, double), long> colors = GetPanel(inputData[0], 0);

            return colors.Count.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            Dictionary<(double, double), long> colors = GetPanel(inputData[0], 1);

            string[] print = new string[200];

            for (int i = 0; i < 200; i++)
            {
                print[i] = "";

                for (int j = 0; j < 200; j++)
                {
                    if (colors.ContainsKey((i - 100, j - 100)))
                    {
                        print[i] += colors[(i - 100, j - 100)] == 1 ? '#' : ' ';
                    }
                    else
                    {
                        print[i] += ' ';
                    }
                }
            }

            foreach (string line in print)
            {
                Console.WriteLine(line);
            }

            return colors.Count.ToString();
        }

        private static Dictionary<(double, double), long> GetPanel(string input, int initialColor)
        {
            long[] program = input.Split(',').Select(long.Parse).ToArray();
            (double, double) currentPosition = (0, 0);
            (double, double) currentDirection = (0, 1);
            Dictionary<(double, double), long> colors = [];

            IntCodeComputer intCodeComputer = new(program);

            colors.Add(currentPosition, initialColor);

            while (!intCodeComputer.IsHalted)
            {
                if (!colors.ContainsKey(currentPosition))
                {
                    colors.Add(currentPosition, 0);
                }

                intCodeComputer.SetInputs([colors[currentPosition]]);

                colors[currentPosition] = intCodeComputer.Run(true);

                currentDirection = intCodeComputer.Run(true) == 0 ?
                    currentDirection.Rotate(Math.PI / 2) :
                    currentDirection.Rotate(-Math.PI / 2);

                currentPosition = currentPosition.Add((Math.Round(currentDirection.Item1), Math.Round(currentDirection.Item2)));
            }

            return colors;
        }
    }
}