using aoc_2019.Utils;
using Main.Tools;

namespace Main.Solutions
{
    public class Day5Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            int[] numbers = inputData[0].Split(',').Select(int.Parse).ToArray();

            IntCodeComputer intCodeComputer = new(numbers);

            int result = 0;

            while (!intCodeComputer.IsHalted)
            {
                intCodeComputer.SetInputs([1]);
                result = intCodeComputer.Run();
            }

            return result.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            int[] numbers = inputData[0].Split(',').Select(int.Parse).ToArray();

            IntCodeComputer intCodeComputer = new(numbers);

            int result = 0;

            while (!intCodeComputer.IsHalted)
            {
                intCodeComputer.SetInputs([5]);
                result = intCodeComputer.Run();
            }

            return result.ToString();
        }
    }
}