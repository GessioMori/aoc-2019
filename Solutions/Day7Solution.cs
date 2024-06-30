using aoc_2019.Utils;
using Main.Tools;

namespace Main.Solutions
{
    public class Day7Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            List<int[]> permutations = GetPermutations([0, 1, 2, 3, 4]);
            long largestOutputSignal = int.MinValue;

            foreach (int[] permSignal in permutations)
            {
                long currentInput = 0;

                foreach (int signal in permSignal)
                {
                    long[] numbers = inputData[0].Split(',').Select(long.Parse).ToArray();

                    IntCodeComputer intCodeComputer = new(numbers);
                    intCodeComputer.SetInputs([signal, currentInput]);

                    currentInput = intCodeComputer.Run();
                }

                if (currentInput > largestOutputSignal)
                {
                    largestOutputSignal = currentInput;
                }
            }

            return largestOutputSignal.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            List<int[]> phasePermutations = GetPermutations([5, 6, 7, 8, 9]);
            long largestOutputSignal = int.MinValue;
            long[] numbers = inputData[0].Split(',').Select(long.Parse).ToArray();

            foreach (int[] phaseNumbers in phasePermutations)
            {
                long currentInput = 0;

                List<IntCodeComputer> computers = phaseNumbers.Select(_ => new IntCodeComputer((long[])numbers.Clone())).ToList();

                bool isFirstLoop = true;

                while (!computers.Last().IsHalted)
                {
                    if (isFirstLoop)
                    {
                        for (int i = 0; i < computers.Count; i++)
                        {
                            computers[i].SetInputs([phaseNumbers[i], currentInput]);
                            currentInput = computers[i].Run();
                        }
                        isFirstLoop = false;
                    }

                    for (int i = 0; i < computers.Count; i++)
                    {
                        computers[i].SetInputs([currentInput]);
                        currentInput = computers[i].Run();
                    }
                }

                if (currentInput > largestOutputSignal)
                {
                    largestOutputSignal = currentInput;
                }
            }

            return largestOutputSignal.ToString();
        }

        static List<int[]> GetPermutations(List<int> list)
        {
            List<int[]> result = [];
            Permute(list, 0, list.Count - 1, result);
            return result;
        }

        static void Permute(List<int> list, int start, int end, List<int[]> result)
        {
            if (start == end)
            {
                result.Add(list.ToArray());
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    Swap(list, start, i);
                    Permute(list, start + 1, end, result);
                    Swap(list, start, i);
                }
            }
        }
        static void Swap(List<int> list, int i, int j)
        {
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}