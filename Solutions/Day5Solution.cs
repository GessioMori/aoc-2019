using Main.Tools;

namespace Main.Solutions
{
    public class Day5Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            int[] numbers = inputData[0].Split(',').Select(int.Parse).ToArray();

            int nextPosition = 4;
            int input = 1;
            int output = 0;

            for (int i = 0; i < numbers.Length; i += nextPosition)
            {
                int opCode = numbers[i];

                int instruction = GetInstructions(opCode, out bool pos1IsImmediate, out bool pos2IsImmediate, out bool pos3IsImmediate);

                if (instruction == 1 || instruction == 2)
                {
                    nextPosition = 4;

                    int arg1Value = pos1IsImmediate ? numbers[i + 1] : numbers[numbers[i + 1]];
                    int arg2Value = pos2IsImmediate ? numbers[i + 2] : numbers[numbers[i + 2]];
                    int resultPosition = pos3IsImmediate ? i + 3 : numbers[i + 3];

                    if (instruction == 1)
                    {
                        numbers[resultPosition] = arg1Value + arg2Value;
                    }
                    else if (instruction == 2)
                    {
                        numbers[resultPosition] = arg1Value * arg2Value;
                    }
                }
                else if (instruction == 3 || instruction == 4)
                {
                    nextPosition = 2;

                    int resultPosition = pos1IsImmediate ? i + 1 : numbers[i + 1];

                    if (instruction == 3)
                    {
                        numbers[resultPosition] = input;
                    }
                    else if (instruction == 4)
                    {
                        output = numbers[resultPosition];
                        if (output != 0)
                        {
                            return output.ToString();
                        }
                    }
                }
            }

            throw new Exception("No response found");
        }

        public string RunPartB(string[] inputData)
        {
            int[] numbers = inputData[0].Split(',').Select(int.Parse).ToArray();

            int input = 5;
            int i = 0;

            while (true)
            {
                int opCode = numbers[i];

                int instruction = GetInstructions(opCode, out bool pos1IsImmediate, out bool pos2IsImmediate, out bool pos3IsImmediate);

                if (instruction == 1 || instruction == 2)
                {
                    int arg1Value = pos1IsImmediate ? numbers[i + 1] : numbers[numbers[i + 1]];
                    int arg2Value = pos2IsImmediate ? numbers[i + 2] : numbers[numbers[i + 2]];
                    int resultPosition = pos3IsImmediate ? i + 3 : numbers[i + 3];

                    if (instruction == 1)
                    {
                        numbers[resultPosition] = arg1Value + arg2Value;
                    }
                    else if (instruction == 2)
                    {
                        numbers[resultPosition] = arg1Value * arg2Value;
                    }

                    i += 4;
                }
                else if (instruction == 3 || instruction == 4)
                {
                    int resultPosition = pos1IsImmediate ? i + 1 : numbers[i + 1];

                    if (instruction == 3)
                    {
                        numbers[resultPosition] = input;
                    }
                    else if (instruction == 4)
                    {
                        if (numbers[resultPosition] != 0)
                        {
                            return numbers[resultPosition].ToString();
                        }
                    }

                    i += 2;
                }
                else if (instruction == 5 || instruction == 6)
                {
                    int arg1Value = pos1IsImmediate ? numbers[i + 1] : numbers[numbers[i + 1]];
                    int arg2Value = pos2IsImmediate ? numbers[i + 2] : numbers[numbers[i + 2]];

                    if ((instruction == 5 && arg1Value != 0) || (instruction == 6 && arg1Value == 0))
                    {
                        i = arg2Value;
                    }
                    else
                    {
                        i += 3;
                    }
                }
                else if (instruction == 7 || instruction == 8)
                {
                    int arg1Value = pos1IsImmediate ? numbers[i + 1] : numbers[numbers[i + 1]];
                    int arg2Value = pos2IsImmediate ? numbers[i + 2] : numbers[numbers[i + 2]];
                    int resultPosition = pos3IsImmediate ? i + 3 : numbers[i + 3];

                    if (instruction == 7)
                    {
                        numbers[resultPosition] = arg1Value < arg2Value ? 1 : 0;
                    }
                    else if (instruction == 8)
                    {
                        numbers[resultPosition] = arg1Value == arg2Value ? 1 : 0;
                    }

                    i += 4;
                }
            }

            throw new Exception("No response found");
        }

        public static int GetInstructions(int opCode, out bool pos1IsImmediate, out bool pos2IsImmediate, out bool pos3IsImmediate)
        {
            pos1IsImmediate = false;
            pos2IsImmediate = false;
            pos3IsImmediate = false;
            int instruction = 0;

            int[] digits = opCode.ToString()
                        .ToCharArray()
                        .Select(c => int.Parse(c.ToString()))
                        .Reverse()
                        .ToArray();

            instruction = digits[0];

            if (digits.Length >= 3)
            {
                pos1IsImmediate = digits[2] == 1;
            }
            if (digits.Length >= 4)
            {
                pos2IsImmediate = digits[3] == 1;
            }
            if (digits.Length == 5)
            {
                pos3IsImmediate = digits[4] == 1;
            }

            return instruction;
        }
    }
}