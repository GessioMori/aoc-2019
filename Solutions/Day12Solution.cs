using Main.Tools;
using System.Text.RegularExpressions;

namespace Main.Solutions
{
    public class Day12Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            List<Moon> moons = [];
            int steps = 1000;

            for (int i = 0; i < 4; i++)
            {
                moons.Add(new Moon(i.ToString(), ParsePosition(inputData[i])));
            }

            for (int i = 0; i < steps; i++)
            {
                foreach (Moon moon in moons)
                {
                    foreach (Moon secondMoon in moons)
                    {
                        if (moon.Name != secondMoon.Name)
                        {
                            moon.ApplyGravity(secondMoon);
                        }
                    }
                }

                foreach (Moon moon in moons)
                {
                    moon.ApplyVelocity();
                }
            }

            int totalEnergy = 0;
            foreach (Moon moon in moons)
            {
                totalEnergy += moon.GetTotalEnergy();
            }

            return totalEnergy.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            List<long> axisCycles = [0, 0, 0];

            for (int i = 0; i < 3; i++)
            {
                int countCycles = 0;
                List<Moon> moons = [];

                for (int j = 0; j < 4; j++)
                {
                    moons.Add(new Moon(j.ToString(), ParsePosition(inputData[j])));
                }

                while (axisCycles.Any(c => c == 0))
                {
                    countCycles++;

                    foreach (Moon moon in moons)
                    {
                        foreach (Moon secondMoon in moons)
                        {
                            if (moon.Name != secondMoon.Name)
                            {
                                moon.ApplyGravity(secondMoon);
                            }
                        }
                    }

                    foreach (Moon moon in moons)
                    {
                        moon.ApplyVelocity();
                    }

                    CheckCoordinates(moons, axisCycles, countCycles);

                }
            }

            return MathUtils.CalculateLCM(axisCycles).ToString();
        }

        private static void CheckCoordinates(List<Moon> moons, List<long> cycles, int cycleNumber)
        {
            for (int i = 0; i < 3; i++)
            {
                if (cycles[i] == 0)
                {
                    bool isRepeated = true;

                    foreach (Moon moon in moons)
                    {
                        isRepeated = isRepeated && moon.position[i] == moon.startingPosition[i] &&
                            moon.velocity[i] == 0;
                    }

                    if (isRepeated)
                    {
                        cycles[i] = cycleNumber;
                    }
                }
            }
        }

        public static int[] ParsePosition(string line)
        {
            Match match = Regex.Match(line, @"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>");

            int[] result = new int[3];
            if (match.Success)
            {
                for (int i = 0; i < 3; i++)
                {
                    result[i] = int.Parse(match.Groups[i + 1].Value);
                }
            }

            return result;
        }
    }

    internal class Moon
    {
        public string Name;
        public int[] position;
        public int[] startingPosition;
        public int[] velocity;

        public Moon(string name, int[] initialPosition)
        {
            this.Name = name;
            this.position = new int[3];
            this.startingPosition = new int[3];
            Array.Copy(initialPosition, this.position, initialPosition.Length);
            Array.Copy(initialPosition, this.startingPosition, initialPosition.Length);
            this.velocity = [0, 0, 0];
        }

        public void ApplyGravity(Moon otherMoon)
        {
            for (int i = 0; i < position.Length; i++)
            {
                this.velocity[i] += this.position[i] < otherMoon.position[i] ? 1 :
                                this.position[i] > otherMoon.position[i] ? -1 : 0;
            }
        }

        public void ApplyVelocity()
        {
            for (int i = 0; i < position.Length; i++)
            {
                this.position[i] += this.velocity[i];
            }
        }

        public int GetTotalEnergy()
        {
            int totalPosition = 0;
            int totalVelocity = 0;
            for (int i = 0; i < position.Length; i++)
            {
                totalPosition += Math.Abs(this.position[i]);
                totalVelocity += Math.Abs(this.velocity[i]);
            }
            return totalVelocity * totalPosition;
        }
    }
}