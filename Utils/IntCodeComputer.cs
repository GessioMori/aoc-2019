namespace aoc_2019.Utils
{
    internal class IntCodeComputer
    {
        private int currentPosition;
        private int[] originalSequence;
        private int[] currentSequence;
        private int currentOutput;
        private bool isHalted;
        private bool isPaused;
        private List<int> inputs;
        private List<int> originalInputs;

        public IntCodeComputer(int[] originalSequence)
        {
            this.originalSequence = originalSequence;
            this.currentSequence = (int[])originalSequence.Clone();
            this.currentPosition = 0;
            this.isHalted = false;
            this.isPaused = false;
            this.inputs = [];
            this.originalInputs = [];
        }

        public bool IsHalted
        {
            get { return this.isHalted; }
            private set { this.isHalted = value; }
        }

        public void SetInputs(int[] inputs)
        {
            this.inputs.Clear();
            this.inputs.AddRange(inputs);
            this.originalInputs.Clear();
            this.originalInputs.AddRange(inputs);
        }

        private void RestoreInputs()
        {
            this.inputs.Clear();
            this.inputs.AddRange(this.originalInputs);
        }

        public int Run()
        {
            this.isPaused = false;

            while (!this.isHalted && !this.isPaused)
            {
                int opCode = this.currentSequence[this.currentPosition];
                int instruction = GetInstructions(opCode, out bool pos1IsImmediate, out bool pos2IsImmediate);

                if (instruction == 1 || instruction == 2)
                {
                    int arg1Value = GetValue(pos1IsImmediate, this.currentPosition + 1);
                    int arg2Value = GetValue(pos2IsImmediate, this.currentPosition + 2);
                    int resultPosition = this.currentSequence[this.currentPosition + 3];

                    if (instruction == 1)
                    {
                        this.currentSequence[resultPosition] = arg1Value + arg2Value;
                    }
                    else if (instruction == 2)
                    {
                        this.currentSequence[resultPosition] = arg1Value * arg2Value;
                    }

                    this.currentPosition += 4;
                }
                else if (instruction == 3 || instruction == 4)
                {
                    int resultPosition = this.currentSequence[this.currentPosition + 1];

                    if (instruction == 3)
                    {
                        int currentInput = this.inputs[0];
                        this.inputs.RemoveAt(0);
                        this.currentSequence[resultPosition] = currentInput;
                    }
                    else if (instruction == 4)
                    {
                        this.currentOutput = this.currentSequence[resultPosition];
                        if (this.currentOutput != 0)
                        {
                            this.isPaused = true;
                        }
                        else
                        {
                            this.RestoreInputs();
                        }
                    }

                    this.currentPosition += 2;
                }
                else if (instruction == 5 || instruction == 6)
                {
                    int arg1Value = GetValue(pos1IsImmediate, this.currentPosition + 1);
                    int arg2Value = GetValue(pos2IsImmediate, this.currentPosition + 2);

                    if ((instruction == 5 && arg1Value != 0) || (instruction == 6 && arg1Value == 0))
                    {
                        this.currentPosition = arg2Value;
                    }
                    else
                    {
                        this.currentPosition += 3;
                    }
                }
                else if (instruction == 7 || instruction == 8)
                {
                    int arg1Value = GetValue(pos1IsImmediate, this.currentPosition + 1);
                    int arg2Value = GetValue(pos2IsImmediate, this.currentPosition + 2);
                    int resultPosition = this.currentSequence[this.currentPosition + 3];

                    if (instruction == 7)
                    {
                        this.currentSequence[resultPosition] = arg1Value < arg2Value ? 1 : 0;
                    }
                    else if (instruction == 8)
                    {
                        this.currentSequence[resultPosition] = arg1Value == arg2Value ? 1 : 0;
                    }

                    this.currentPosition += 4;
                }

                opCode = this.currentSequence[this.currentPosition];

                if (opCode == 99)
                {
                    this.isHalted = true;
                }
            }

            return this.currentOutput;
        }

        private int GetValue(bool isImmediate, int position)
        {
            return isImmediate ? this.currentSequence[position] : this.currentSequence[this.currentSequence[position]];
        }

        private int GetInstructions(int opCode, out bool pos1IsImmediate, out bool pos2IsImmediate)
        {
            pos1IsImmediate = false;
            pos2IsImmediate = false;
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

            return instruction;
        }
    }
}
