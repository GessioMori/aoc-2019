using Main.Tools;

namespace Main.Solutions
{
    public class Day4Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            int min = int.Parse(inputData[0].Split('-')[0]);
            int max = int.Parse(inputData[0].Split('-')[1]);

            int possibles = 0;

            for (int i = min; i <= max; i++)
            {
                bool isPossible = true;

                int[] digits = i.ToString()
                        .ToCharArray()
                        .Select(c => int.Parse(c.ToString()))
                        .ToArray();

                for (int j = 0; j < digits.Length - 1; j++)
                {
                    if (digits[j] > digits[j + 1])
                    {
                        isPossible = false;
                        break;
                    }
                }

                if (isPossible)
                {
                    HashSet<int> uniqueDigits = new(digits);

                    if (uniqueDigits.Count != digits.Length)
                    {
                        possibles++;
                    }
                }
            }

            return possibles.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            int min = int.Parse(inputData[0].Split('-')[0]);
            int max = int.Parse(inputData[0].Split('-')[1]);

            int possibles = 0;

            for (int i = min; i <= max; i++)
            {
                bool isPossible = true;

                int[] digits = i.ToString()
                        .ToCharArray()
                        .Select(c => int.Parse(c.ToString()))
                        .ToArray();



                for (int j = 0; j < digits.Length - 1; j++)
                {
                    if (digits[j] > digits[j + 1])
                    {
                        isPossible = false;
                        break;
                    }
                }

                if (isPossible)
                {
                    HashSet<int> uniqueDigits = new(digits);

                    if (uniqueDigits.Count != digits.Length)
                    {
                        Dictionary<int, int> dict = [];

                        foreach (int d in digits)
                        {
                            if (dict.ContainsKey(d))
                            {
                                dict[d]++;
                            }
                            else
                            {
                                dict.Add(d, 1);
                            }
                        }

                        if (dict.ContainsValue(2))
                        {
                            possibles++;
                        }
                    }
                }
            }

            return possibles.ToString();
        }
    }
}