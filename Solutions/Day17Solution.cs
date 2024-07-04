using aoc_2019.Utils;
using Main.Tools;
using System.Text.RegularExpressions;

namespace Main.Solutions
{
    public class Day17Solution : ISolution
    {
        private readonly Dictionary<int, string> symbols = new Dictionary<int, string> {
            {35, "#" },
            {46, "." },
            {10, char.ConvertFromUtf32(10) },
            {60, "<" },
            {62, ">" },
            {94, "^" },
            {86, "v" },
            {88, "X" },
        };

        private readonly List<(int, int)> possibleMovs = [(0, -1), (0, 1), (-1, 0), (1, 0)];

        private IntCodeComputer? computer;

        public string RunPartA(string[] inputData)
        {
            long[] program = inputData[0].Split(',').Select(long.Parse).ToArray();

            Dictionary<(int, int), string> map = CreateMap(program, false);

            Dictionary<(int, int), string> intersections = GetIntersections(map);

            int result = 0;

            foreach (KeyValuePair<(int, int), string> intersection in intersections)
            {
                result += intersection.Key.Item1 * intersection.Key.Item2;
            }

            return result.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            long[] program = inputData[0].Split(',').Select(long.Parse).ToArray();

            Dictionary<(int, int), string> map = CreateMap(program, true);

            Dictionary<(int, int), List<(int, int)>> adjacencyList = GetAdjacencyList(map);

            KeyValuePair<(int, int), string> startNode = map.First(kvp => kvp.Value != "." && kvp.Value != "#");

            List<string> path = FindPath(startNode, adjacencyList);

            List<List<string>> repeatingSubstrings = GetAllSublists(path);

            List<List<string>> groups = [];

            for (int i = 0; i < repeatingSubstrings.Count; i++)
            {
                for (int j = i + 1; j < repeatingSubstrings.Count; j++)
                {
                    for (int k = j + 1; k < repeatingSubstrings.Count; k++)
                    {
                        if (CanComposeOriginalList(path, [repeatingSubstrings[i], repeatingSubstrings[j], repeatingSubstrings[k]]))
                        {
                            groups.AddRange([repeatingSubstrings[i], repeatingSubstrings[j], repeatingSubstrings[k]]);
                        }
                    }
                }
            }

            string stringPath = string.Join("", path);
            Dictionary<string, string> stringGroups = [];

            stringGroups.Add("A", string.Join("", groups[0]));
            stringGroups.Add("B", string.Join("", groups[1]));
            stringGroups.Add("C", string.Join("", groups[2]));

            List<string> orderedGroups = GetGroupsOrder(stringPath, stringGroups);

            string newLineChar = char.ConvertFromUtf32(10);
            string mainRoutine = string.Join(",", orderedGroups) + newLineChar;
            string functionA = string.Join(',', groups[0]) + newLineChar;
            string functionB = string.Join(',', groups[1]) + newLineChar;
            string functionC = string.Join(',', groups[2]) + newLineChar;
            string continousVideoResponse = "n" + newLineChar;

            Console.WriteLine("Main routine: " + mainRoutine);
            Console.WriteLine("A: " + functionA);
            Console.WriteLine("B: " + functionB);
            Console.WriteLine("C: " + functionC);

            long[] robotProgram = inputData[0].Split(',').Select(long.Parse).Select((e, i) => i == 0 ? 2 : e).ToArray();

            IntCodeComputer robotComputer = new(robotProgram);

            string fullInput = mainRoutine + functionA + functionB + functionC + continousVideoResponse;

            robotComputer.SetInputs([.. GetASCIICodes(fullInput)]);

            long result = 0;

            while (!robotComputer.IsHalted)
            {
                result = robotComputer.Run();
            }

            return result.ToString();
        }

        private List<long> GetASCIICodes(string input)
        {
            List<long> list = [];

            foreach (char c in input)
            {
                list.Add(Convert.ToInt32(c));
            }

            return list;
        }

        private List<string> GetGroupsOrder(string original, Dictionary<string, string> groups)
        {
            List<(string Group, int Position)> matches = [];

            for (int i = 0; i < groups.Count; i++)
            {
                Regex regex = new(Regex.Escape(groups.Values.ToList()[i]));

                foreach (Match match in regex.Matches(original).Cast<Match>())
                {
                    matches.Add((groups.Keys.ToList()[i], match.Index));
                }
            }

            matches.Sort((x, y) => x.Position.CompareTo(y.Position));

            List<string> orderedGroups = [];

            foreach ((string Group, int Position) match in matches)
            {
                orderedGroups.Add(match.Group);
            }

            return orderedGroups;
        }

        static List<List<string>> GetAllSublists(List<string> list)
        {
            HashSet<List<string>> uniqueSublists = new(new ListComparer());
            int n = list.Count;

            for (int len = 1; len <= n; len++)
            {
                for (int start = 0; start <= n - len; start++)
                {
                    List<string> sublist = [];

                    for (int i = 0; i < len; i++)
                    {
                        sublist.Add(list[start + i]);
                    }

                    uniqueSublists.Add(sublist);
                }
            }

            List<List<string>> filteredList = [];

            foreach (List<string> sublist in uniqueSublists)
            {
                string joined = string.Join(",", sublist);

                if (joined != null && joined.Length <= 20)
                {
                    filteredList.Add(sublist);
                }
            }

            return filteredList;
        }

        static bool CanComposeOriginalList(List<string> originalList, List<List<string>> sublists)
        {
            return CanCompose(originalList, sublists, 0);
        }

        static bool CanCompose(List<string> originalList, List<List<string>> sublists, int startIndex)
        {
            if (startIndex == originalList.Count)
            {
                return true;
            }

            foreach (List<string> sublist in sublists)
            {
                if (IsMatch(originalList, sublist, startIndex))
                {
                    if (CanCompose(originalList, sublists, startIndex + sublist.Count))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool IsMatch(List<string> originalList, List<string> sublist, int startIndex)
        {
            if (startIndex + sublist.Count > originalList.Count)
            {
                return false;
            }

            for (int i = 0; i < sublist.Count; i++)
            {
                if (originalList[startIndex + i] != sublist[i])
                {
                    return false;
                }
            }

            return true;
        }

        private List<string> FindPath(KeyValuePair<(int, int), string> startNode,
            Dictionary<(int, int), List<(int, int)>> adjacencyList)
        {
            (int, int) currentDirection = (0, 0);

            (int, int) currentTile = startNode.Key;
            (int, int) previousTile = (0, 0);

            switch (startNode.Value)
            {
                case "^":
                    currentDirection = (0, -1);
                    break;
                case "v":
                    currentDirection = (0, 1);
                    break;
                case "<":
                    currentDirection = (-1, 0);
                    break;
                case ">":
                    currentDirection = (1, 0);
                    break;
            }

            List<string> path = [];

            bool isPathAvailable = true;

            while (isPathAvailable)
            {
                (bool canMoveForward, (int, int) nextTile) = CanMoveForward(currentTile, currentDirection, adjacencyList);

                if (canMoveForward)
                {
                    if (int.TryParse(path.Last(), out int steps))
                    {
                        path[path.Count - 1] = (steps + 1).ToString();
                    }
                    else
                    {
                        path.Add("1");
                    }

                    previousTile = currentTile;
                    currentTile = nextTile;
                }
                else
                {
                    (string dirChar, (int, int) newDirection) = GetDirectionToTurnRobot(currentTile,
                        currentDirection, adjacencyList, previousTile);

                    if (dirChar != "X")
                    {
                        path.Add(dirChar);
                        currentDirection = newDirection;
                    }
                    else
                    {
                        isPathAvailable = false;
                    }
                }
            }


            return path;
        }

        private (bool, (int, int)) CanMoveForward((int, int) currentNode, (int, int) currentDirection,
            Dictionary<(int, int), List<(int, int)>> adjacencyList)
        {
            (int, int) nextNode = currentNode.Add(currentDirection);

            return (adjacencyList[currentNode].Contains(nextNode), nextNode);
        }

        private (string, (int, int)) GetDirectionToTurnRobot((int, int) currentNode, (int, int) currentDirection,
            Dictionary<(int, int), List<(int, int)>> adjacencyList, (int, int) previousNode)
        {
            (int, int) nextNode = adjacencyList[currentNode].FirstOrDefault(n => n != previousNode);

            (int, int) direction = (nextNode.Item1 - currentNode.Item1,
                nextNode.Item2 - currentNode.Item2);

            string directionChar = direction switch
            {
                (1, 0) => currentDirection == (0, -1) ? "R" : currentDirection == (0, 1) ? "L" : "X",
                (-1, 0) => currentDirection == (0, -1) ? "L" : currentDirection == (0, 1) ? "R" : "X",
                (0, -1) => currentDirection == (-1, 0) ? "R" : currentDirection == (1, 0) ? "L" : "X",
                (0, 1) => currentDirection == (-1, 0) ? "L" : currentDirection == (1, 0) ? "R" : "X",
                _ => "X"
            };

            return (directionChar, direction);
        }

        private Dictionary<(int, int), string> GetIntersections(Dictionary<(int, int), string> map)
        {
            Dictionary<(int, int), string> intersections = [];

            foreach (KeyValuePair<(int, int), string> tile in map)
            {
                int validNeighbours = 0;

                for (int i = 0; i < 4; i++)
                {
                    (int, int) neighbourPos = tile.Key.Add(this.possibleMovs[i]);


                    if (map.TryGetValue(neighbourPos, out string? value) && value == "#")
                    {
                        validNeighbours++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (validNeighbours == 4)
                {
                    intersections.Add(tile.Key, tile.Value);
                }
            }

            return intersections;
        }

        private Dictionary<(int, int), List<(int, int)>> GetAdjacencyList(Dictionary<(int, int), string> map)
        {
            Dictionary<(int, int), List<(int, int)>> adjacencyList = [];

            foreach (KeyValuePair<(int, int), string> tile in map)
            {
                if (tile.Value != ".")
                {
                    adjacencyList.Add(tile.Key, []);

                    for (int i = 0; i < 4; i++)
                    {
                        (int, int) neighbourPos = tile.Key.Add(this.possibleMovs[i]);

                        if (map.TryGetValue(neighbourPos, out string? value) && value != ".")
                        {
                            adjacencyList[tile.Key].Add(neighbourPos);
                        }
                    }
                }
            }

            return adjacencyList;
        }

        private Dictionary<(int, int), string> CreateMap(long[] program, bool printMap)
        {
            this.computer = new(program);

            List<string> stringMap = [];
            stringMap.Add("");

            Dictionary<(int, int), string> map = [];

            int currentRow = 0;
            int currentColumn = 0;

            while (!this.computer.IsHalted)
            {
                int code = (int)this.computer.Run();

                if (code == 10)
                {
                    currentRow++;
                    currentColumn = 0;
                    stringMap.Add("");
                }
                else
                {
                    map.Add((currentColumn, currentRow), char.ConvertFromUtf32(code));
                    stringMap[currentRow] += char.ConvertFromUtf32(code);
                    currentColumn++;
                }
            }

            if (printMap)
            {
                foreach (string row in stringMap)
                {
                    Console.WriteLine(row);
                }
            }

            return map;
        }
    }
}