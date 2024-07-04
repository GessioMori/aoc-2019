using Main.Tools;

namespace Main.Solutions
{
    public class Day16Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            int[] currentPhase = ParseInput(inputData[0]);

            int inputLength = currentPhase.Length;

            int[] pattern = [0, 1, 0, -1];

            int[] nextPhase = new int[inputLength];

            int phases = 100;

            Array.Copy(currentPhase, nextPhase, inputLength);

            for (int p = 1; p <= phases; p++)
            {
                for (int i = 0; i < inputLength; i++)
                {
                    int newElement = 0;

                    for (int j = 0; j < inputLength; j++)
                    {
                        newElement += currentPhase[j] * pattern[(int)Math.Floor((double)(j + 1) / (i + 1)) % 4];
                    }

                    newElement = Math.Abs(newElement % 10);

                    nextPhase[i] = newElement;
                }

                Array.Copy(nextPhase, currentPhase, inputLength);
            }

            return string.Join("", nextPhase)[0..8];
        }

        public string RunPartB(string[] inputData)
        {
            int[] originalInput = ParseInput(inputData[0]);
            int originalLength = originalInput.Length;
            int timesRepeated = 10000;
            int repeatedLength = originalLength * timesRepeated;
            int elementsToSkip = int.Parse(string.Join("", originalInput)[0..7]);

            int[] invertedInput = new int[originalLength];

            for (int i = 0; i < originalLength; i++)
            {
                invertedInput[originalLength - i - 1] = originalInput[i];
            }

            int[] copiedInput = new int[repeatedLength];

            for (int i = 0; i < timesRepeated; i++)
            {
                Array.Copy(invertedInput, 0, copiedInput, i * originalLength, originalLength);
            }

            int numOfElementsToGet = repeatedLength - elementsToSkip;

            int[][] phases = new int[101][];

            for (int i = 0; i <= 100; i++)
            {
                phases[i] = new int[numOfElementsToGet];
            }

            for (int i = 0; i < numOfElementsToGet; i++)
            {
                phases[0][i] = copiedInput[i];
            }

            for (int p = 1; p < 101; p++)
            {
                phases[p][0] = phases[p - 1][0];
            }

            for (int i = 1; i < numOfElementsToGet; i++)
            {
                for (int p = 1; p < 101; p++)
                {
                    phases[p][i] = (phases[p - 1][i] + phases[p][i - 1]) % 10;
                }
            }

            int[] lastElements = phases[100].Skip(phases[100].Length - 8).Take(8).ToArray();
            string concatenated = string.Join("", lastElements);
            char[] charArray = concatenated.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

        private int GetFromPreviousPhase(int[][] phases, int phaseNumber, int index)
        {
            int result = 0;

            for (int i = index; i >= 0; i--)
            {
                result += phases[phaseNumber - 1][i];
            }

            return result % 10;
        }

        private int[] ParseInput(string line)
        {
            return line.Select(c => int.Parse(c.ToString())).ToArray();
        }
    }
}