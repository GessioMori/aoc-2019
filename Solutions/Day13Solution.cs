using aoc_2019.Utils;
using Main.Tools;

namespace Main.Solutions
{
    public class Day13Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            long[] program = inputData[0].Split(',').Select(long.Parse).ToArray();
            Dictionary<(int, int), int> tiles = CreateGame(program);

            return tiles.Values.Count(t => t == 2).ToString();
        }

        public string RunPartB(string[] inputData)
        {
            long[] program = inputData[0].Split(',').Select(long.Parse).Select((e, i) => i == 0 ? 2 : e).ToArray();

            Dictionary<(int, int), int> tiles = [];

            int joystick = 0;
            int score = 0;

            IntCodeComputer computer = new(program);

            while (!computer.IsHalted)
            {
                computer.SetInputs([joystick]);

                int firstOutput = (int)computer.Run(true);
                int secondOutput = (int)computer.Run(true);
                int thirdOutput = (int)computer.Run(true);

                if (firstOutput == -1 && secondOutput == 0)
                {
                    score = thirdOutput;
                }
                else if (!tiles.ContainsKey((firstOutput, secondOutput)))
                {
                    tiles.Add((firstOutput, secondOutput), thirdOutput);
                }
                else
                {
                    tiles[(firstOutput, secondOutput)] = thirdOutput;
                }

                int ball = tiles.FirstOrDefault(kvp => kvp.Value == 4).Key.Item1;
                int paddle = tiles.FirstOrDefault(kvp => kvp.Value == 3).Key.Item1;

                if (ball > paddle)
                {
                    joystick = 1;
                }
                else if (ball < paddle)
                {
                    joystick = -1;
                }
                else
                {
                    joystick = 0;
                }
            }

            return score.ToString();
        }

        private Dictionary<(int, int), int> CreateGame(long[] program)
        {
            Dictionary<(int, int), int> tiles = [];

            IntCodeComputer computer = new(program);

            while (!computer.IsHalted)
            {
                int[] tile = [(int)computer.Run(true), (int)computer.Run(true), (int)computer.Run(true)];
                if (!tiles.ContainsKey((tile[0], tile[1])))
                {
                    tiles.Add((tile[0], tile[1]), tile[2]);
                }
            }

            return tiles;
        }
    }
}