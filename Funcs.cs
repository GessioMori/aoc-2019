using System.Text.RegularExpressions;

namespace Main.Tools
{
    public static class Funcs
    {
        public static string[] ReadFileToArray(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            return File.ReadAllLines(filePath);
        }

        public static ISolution CreateSolutionInstance(int day)
        {
            string typeName = $"Main.Solutions.Day{day}Solution";
            Type? solutionType = Type.GetType(typeName);
            if (solutionType != null)
            {
                object? solution = Activator.CreateInstance(solutionType);
                if (solution != null)
                {
                    return (ISolution)solution;
                }
            }

            throw new InvalidOperationException($"Solution type '{typeName}' not found.");
        }

        public static void RunExampleAndCheck(string[] exampleData, char partChoice, int day)
        {
            ISolution solution = CreateSolutionInstance(day);

            List<string[]> splitExampleData = new List<string[]>();
            List<string> currentStrings = new List<string>();

            foreach (string line in exampleData)
            {
                if (line.Equals("==="))
                {
                    splitExampleData.Add(currentStrings.ToArray());
                    currentStrings.Clear();
                }
                else
                {
                    currentStrings.Add(line);
                }
            }

            if (currentStrings.Count > 0)
            {
                splitExampleData.Add(currentStrings.ToArray());
            }

            string expectedAnswer = partChoice == 'a' ? splitExampleData[1][0] : splitExampleData[3][0];
            string[] exampleDataInput = partChoice == 'a' ? splitExampleData[0] : splitExampleData[2];
            string result = partChoice == 'a' ? solution.RunPartA(exampleDataInput) : solution.RunPartB(exampleDataInput);

            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"Expected: {expectedAnswer}");
            Console.WriteLine($"Test passed: {result == expectedAnswer}");
        }

        public static void RunSolution(string[] inputData, char partChoice, int day)
        {
            ISolution solution = CreateSolutionInstance(day);
            string result = partChoice == 'a' ? solution.RunPartA(inputData) : solution.RunPartB(inputData);

            Console.WriteLine($"Result: {result}");
        }

        public static bool ValidateArgs(string[] args, out int day, out char testOrInput, out char challengePart)
        {
            testOrInput = 'i';
            challengePart = 'a';
            day = 0;
            if (args.Length > 0)
            {
                Match match = Regex.Match(args[0], @"(\d+)(t|i)(a|b)");

                if (match.Success)
                {
                    day = int.Parse(match.Groups[1].Value);
                    testOrInput = char.Parse(match.Groups[2].Value);
                    challengePart = char.Parse(match.Groups[3].Value);

                    return true;
                }
            }

            return false;
        }
    }

    public static class MathUtils
    {
        public static long LCM(long a, long b)
        {
            return (a / GCD(a, b)) * b;
        }

        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long CalculateLCM(List<long> numbers)
        {
            return numbers.Aggregate(LCM);
        }
    }

    public interface ISolution
    {
        string RunPartA(string[] inputData);
        string RunPartB(string[] inputData);
    }

    public static class TupleExtensions
    {
        public static (int, int) Add(this (int, int) tuple1, (int, int) tuple2)
        {
            return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
        }

        public static (double, double) Add(this (double, double) tuple1, (double, double) tuple2)
        {
            return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
        }

        public static (int, int) GetDirection(this (int, int) tuple)
        {
            int x = tuple.Item1;
            int y = tuple.Item2;

            return (x != 0 ? x / Math.Abs(x) : 0, y != 0 ? y / Math.Abs(y) : 0);
        }

        public static bool Equals(this (int, int) tuple1, (int, int) tuple2)
        {
            return tuple1.Item1 == tuple2.Item1 && tuple1.Item2 == tuple2.Item2;
        }

        public static double Distance(this (int, int) tuple1, (int, int) tuple2)
        {
            return Math.Sqrt(Math.Pow(tuple1.Item1 - tuple2.Item1, 2) + Math.Pow(tuple1.Item2 - tuple2.Item2, 2));
        }

        public static (double, double) Rotate(this (double, double) tuple, double angle)
        {
            return (tuple.Item1 * Math.Cos(angle) - tuple.Item2 * Math.Sin(angle),
                tuple.Item1 * Math.Sin(angle) + tuple.Item2 * Math.Cos(angle));
        }
    }

    class ListComparer : IEqualityComparer<List<string>>
    {
        public bool Equals(List<string>? x, List<string>? y)
        {
            if (x == null || y == null) return false;
            if (x.Count != y.Count) return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public int GetHashCode(List<string> obj)
        {
            int hash = 19;
            foreach (var item in obj)
            {
                hash = hash * 31 + (item == null ? 0 : item.GetHashCode());
            }
            return hash;
        }
    }
}