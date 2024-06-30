namespace aoc_2019.Utils
{
    internal class IntCodeComputer
    {
        private long currentPosition;
        private long[] currentSequence;
        private long currentOutput;
        private bool isHalted;
        private bool isPaused;
        private List<long> inputs;
        private List<long> originalInputs;
        private long relativeBase;

        public IntCodeComputer(long[] originalSequence)
        {
            this.currentSequence = new long[originalSequence.Length + 10000];
            Array.Copy(originalSequence, this.currentSequence, originalSequence.Length);
            this.currentPosition = 0;
            this.relativeBase = 0;
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

        public void SetInputs(long[] inputs)
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

        public long Run()
        {
            this.isPaused = false;

            while (!this.isHalted && !this.isPaused)
            {
                long opCode = this.currentSequence[this.currentPosition];
                int instruction = GetInstructions(opCode, out InstructionMode pos1Mode, 
                    out InstructionMode pos2Mode, out InstructionMode pos3Mode);

                if (instruction == 1 || instruction == 2)
                {
                    long arg1Value = GetValue(pos1Mode, this.currentPosition + 1);
                    long arg2Value = GetValue(pos2Mode, this.currentPosition + 2);
                    long resultPosition = GetPosition(pos3Mode, this.currentPosition + 3);

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
                else if (instruction == 3)
                {
                    long resultPosition = GetPosition(pos1Mode, this.currentPosition + 1);
                    long currentInput = this.inputs[0];
                    this.inputs.RemoveAt(0);
                    this.currentSequence[resultPosition] = currentInput;
                    this.currentPosition += 2;
                }
                else if (instruction == 4)
                {
                    this.currentOutput = GetValue(pos1Mode, this.currentPosition + 1);
                    if (this.currentOutput != 0)
                    {
                        this.isPaused = true;
                    }
                    else
                    {
                        this.RestoreInputs();
                    }
                    this.currentPosition += 2;
                }
                else if (instruction == 5 || instruction == 6)
                {
                    long arg1Value = GetValue(pos1Mode, this.currentPosition + 1);
                    long arg2Value = GetValue(pos2Mode, this.currentPosition + 2);

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
                    long arg1Value = GetValue(pos1Mode, this.currentPosition + 1);
                    long arg2Value = GetValue(pos2Mode, this.currentPosition + 2);
                    long resultPosition = this.GetPosition(pos3Mode, this.currentPosition + 3);

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
                else if (instruction == 9)
                {
                    long arg1Value = GetValue(pos1Mode, this.currentPosition + 1);
                    this.relativeBase += arg1Value;
                    this.currentPosition += 2;
                }

                opCode = this.currentSequence[this.currentPosition];

                if (opCode == 99)
                {
                    this.isHalted = true;
                }
            }

            return this.currentOutput;
        }

        private long GetValue(InstructionMode instructionMode, long position)
        {
            switch (instructionMode)
            {
                case InstructionMode.PositionalMode:
                    return this.currentSequence[this.currentSequence[position]];
                case InstructionMode.ImmediateMode:
                    return this.currentSequence[position];
                case InstructionMode.RelativeMode:
                    return this.currentSequence[this.currentSequence[position] + this.relativeBase];
            }

            return this.currentSequence[position];
        }

        private long GetPosition(InstructionMode instructionMode, long position)
        {
            switch (instructionMode)
            {
                case InstructionMode.PositionalMode:
                    return this.currentSequence[position];
                case InstructionMode.ImmediateMode:
                    return position;
                case InstructionMode.RelativeMode:
                    return this.currentSequence[position] + this.relativeBase;
            }

            return this.currentSequence[position];
        }

        private int GetInstructions(long opCode, out InstructionMode pos1Mode, out InstructionMode pos2Mode, out InstructionMode pos3Mode)
        {
            pos1Mode = InstructionMode.PositionalMode;
            pos2Mode = InstructionMode.PositionalMode;
            pos3Mode = InstructionMode.PositionalMode;

            int instruction = 0;

            int[] digits = opCode.ToString()
                        .ToCharArray()
                        .Select(c => int.Parse(c.ToString()))
                        .Reverse()
                        .ToArray();

            instruction = digits[0];

            if (digits.Length >= 3)
            {
                pos1Mode = (InstructionMode)Enum.ToObject(typeof(InstructionMode), digits[2]);
            }
            if (digits.Length >= 4)
            {
                pos2Mode = (InstructionMode)Enum.ToObject(typeof(InstructionMode), digits[3]);
            }
            if(digits.Length >= 5)
            {
                pos3Mode = (InstructionMode)Enum.ToObject(typeof(InstructionMode), digits[4]);
            }

            return instruction;
        }

        private enum InstructionMode
        {
            PositionalMode = 0,
            ImmediateMode = 1,
            RelativeMode = 2,
        }
    }
}
