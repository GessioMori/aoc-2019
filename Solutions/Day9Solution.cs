using aoc_2019.Utils;
using Main.Tools;

namespace Main.Solutions
{
    public class Day9Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            long[] numbers = inputData[0].Split(',').Select(long.Parse).ToArray();

            IntCodeComputer intCodeComputer = new(numbers);

            long result = 0;

            while (!intCodeComputer.IsHalted)
            {
                intCodeComputer.SetInputs([1]);
                result = intCodeComputer.Run();
            }

            return result.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            long[] numbers = inputData[0].Split(',').Select(long.Parse).ToArray();

            IntCodeComputer intCodeComputer = new(numbers);

            long result = 0;

            while (!intCodeComputer.IsHalted)
            {
                intCodeComputer.SetInputs([2]);
                result = intCodeComputer.Run();
            }

            return result.ToString();
        }
    }
}