using Main.Tools;

namespace Main.Solutions
{
    public class Day1Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            long total = 0;

            foreach (string line in inputData)
            {
                total += int.Parse(line) / 3 - 2;
            }

            return total.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            long total = 0;

            foreach (string line in inputData)
            {
                int aditionalFuel = int.Parse(line) / 3 - 2;

                while (aditionalFuel > 0)
                {
                    total += aditionalFuel;
                    aditionalFuel = aditionalFuel / 3 - 2;
                }
            }

            return total.ToString();
        }
    }
}