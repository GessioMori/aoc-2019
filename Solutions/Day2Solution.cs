using Main.Tools;

namespace Main.Solutions
{
    public class Day2Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            int[] numbers = inputData[0].Split(',').Select(int.Parse).ToArray();

            return FindFirst(numbers, 12, 2).ToString();
        }

        public string RunPartB(string[] inputData)
        {

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int[] numbers = inputData[0].Split(',').Select(int.Parse).ToArray();

                    if (FindFirst(numbers, i, j) == 19690720)
                    {
                        return (100 * i + j).ToString();
                    }
                }
            }

            throw new Exception("No solution found");
        }

        private static int FindFirst(int[] numbers, int a, int b)
        {
            numbers[1] = a;
            numbers[2] = b;

            for (int i = 0; i < numbers.Length; i += 4)
            {
                if (numbers[i] == 1)
                {
                    numbers[numbers[i + 3]] = numbers[numbers[i + 1]] + numbers[numbers[i + 2]];
                }
                else if (numbers[i] == 2)
                {
                    numbers[numbers[i + 3]] = numbers[numbers[i + 1]] * numbers[numbers[i + 2]];
                }
                else
                {
                    break;
                }
            }

            return numbers[0];
        }
    }
}