using Main.Tools;

namespace Main.Solutions
{
    public class Day3Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            (int, int)[] positionsA = GetPoints(ParseMovs(inputData[0]));
            (int, int)[] positionsB = GetPoints(ParseMovs(inputData[1]));

            List<(int, int)> intersections = GetIntersections(positionsA, positionsB);

            return intersections.Select(i => Math.Abs(i.Item1) + Math.Abs(i.Item2)).Min().ToString();
        }

        public string RunPartB(string[] inputData)
        {
            (int, int)[] positionsA = GetPoints(ParseMovs(inputData[0]));
            (int, int)[] positionsB = GetPoints(ParseMovs(inputData[1]));

            List<(int, int)> intersections = GetIntersections(positionsA, positionsB);

            int finalSum = int.MaxValue;

            foreach ((int, int) intersection in intersections)
            {
                int lowestSum = 0;

                foreach ((int, int) pos in positionsA)
                {
                    lowestSum++;
                    if (pos == intersection)
                    {
                        break;
                    }
                }
                foreach ((int, int) pos in positionsB)
                {
                    lowestSum++;
                    if (pos == intersection)
                    {
                        break;
                    }
                }

                if (lowestSum < finalSum)
                {
                    finalSum = lowestSum;
                }
            }

            return finalSum.ToString();
        }

        static bool ContainsTuple((int, int)[] array, (int, int) target)
        {
            foreach (var tuple in array)
            {
                if (tuple.Item1 == target.Item1 && tuple.Item2 == target.Item2)
                {
                    return true;
                }
            }
            return false;
        }

        public static (int, int)[] ParseMovs(string line)
        {
            return line.Split(',').Select(l =>
            {
                char dir = l[0];

                if (dir == 'R') return (int.Parse(l[1..]), 0);
                if (dir == 'L') return (-int.Parse(l[1..]), 0);
                if (dir == 'U') return (0, int.Parse(l[1..]));
                if (dir == 'D') return (0, -int.Parse(l[1..]));
                return (0, 0);
            }).ToArray();
        }

        public static (int, int)[] GetPoints((int, int)[] movs)
        {
            List<(int, int)> points = [];

            (int, int) currentPos = (0, 0);

            foreach ((int, int) mov in movs)
            {
                (int, int) finalPos = currentPos.Add(mov);

                (int, int) dir = mov.GetDirection();

                while (currentPos != finalPos)
                {
                    currentPos = currentPos.Add(dir);
                    points.Add(currentPos);
                }
            }

            return points.ToArray();
        }

        public static List<(int, int)> GetIntersections((int, int)[] wireA, (int, int)[] wireB)
        {
            List<(int, int)> intersections = [];

            foreach ((int, int) posB in wireB)
            {
                if (ContainsTuple(wireA, posB))
                {
                    intersections.Add(posB);
                }
            }

            return intersections;
        }
    }
}