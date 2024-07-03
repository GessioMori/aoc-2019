using aoc_2019.Utils;
using Main.Tools;

namespace Main.Solutions
{
    public class Day15Solution : ISolution
    {
        private IntCodeComputer? computer;
        private readonly List<(int, int)> possibleMovs = [(0, -1), (0, 1), (-1, 0), (1, 0)];
        public string RunPartA(string[] inputData)
        {
            long[] program = inputData[0].Split(',').Select(long.Parse).ToArray();

            Dictionary<(int, int), int> map = CreateMap(program);
            //PrintMap(map);

            Dictionary<(int, int), List<(int, int)>> adjacencyList = CreateAdjacencyList(map);

            (int, int) startPosition = (0, 0);
            (int, int) endPosition = map.First(kvp => kvp.Value == 2).Key;

            List<(int, int)> visitedTiles = [];
            Queue<(int, int)> queue = new();
            Dictionary<(int, int), int> distances = [];
            distances.Add(startPosition, 0);

            visitedTiles.Add(startPosition);
            queue.Enqueue(startPosition);

            while (queue.Count > 0)
            {
                (int, int) currentPosition = queue.Dequeue();

                foreach ((int, int) nextPosition in adjacencyList[currentPosition])
                {
                    if (!visitedTiles.Contains(nextPosition))
                    {
                        distances.Add(nextPosition, distances[currentPosition] + 1);
                        visitedTiles.Add(nextPosition);
                        queue.Enqueue(nextPosition);

                        if (nextPosition == endPosition)
                        {
                            return distances[nextPosition].ToString();
                        }
                    }
                }
            }

            throw new Exception("Oxygen not found!");
        }

        public string RunPartB(string[] inputData)
        {
            long[] program = inputData[0].Split(',').Select(long.Parse).ToArray();

            Dictionary<(int, int), int> map = CreateMap(program);

            Dictionary<(int, int), List<(int, int)>> adjacencyList = CreateAdjacencyList(map);
            PrintMap(map);

            (int, int) endPosition = map.First(kvp => kvp.Value == 2).Key;

            List<(int, int)> visitedTiles = [];
            Queue<(int, int)> queue = new();
            Dictionary<(int, int), int> distances = [];
            distances.Add(endPosition, 0);

            visitedTiles.Add(endPosition);
            queue.Enqueue(endPosition);

            while (queue.Count > 0)
            {
                (int, int) currentPosition = queue.Dequeue();

                foreach ((int, int) nextPosition in adjacencyList[currentPosition])
                {
                    if (!visitedTiles.Contains(nextPosition))
                    {
                        if (distances.TryGetValue(nextPosition, out int d))
                        {
                            if (distances[currentPosition] + 1 < d)
                            {
                                distances[nextPosition] = distances[currentPosition] + 1;
                            }
                        }
                        else
                        {
                            distances.Add(nextPosition, distances[currentPosition] + 1);
                        }
                        visitedTiles.Add(nextPosition);
                        queue.Enqueue(nextPosition);
                    }
                }
            }

            int maxDistance = distances.Max(kvp => kvp.Value);

            return maxDistance.ToString();
        }

        private Dictionary<(int, int), List<(int, int)>> CreateAdjacencyList(Dictionary<(int, int), int> map)
        {
            Dictionary<(int, int), List<(int, int)>> adjacencyList = [];

            foreach (KeyValuePair<(int, int), int> tile in map)
            {
                if (tile.Value != 0)
                {
                    adjacencyList.Add(tile.Key, []);

                    for (int i = 0; i < this.possibleMovs.Count; i++)
                    {
                        (int, int) newPosition = tile.Key.Add(this.possibleMovs[i]);

                        if (map.TryGetValue(newPosition, out int value))
                        {
                            if (value != 0)
                            {
                                adjacencyList[tile.Key].Add(newPosition);
                            }
                        }
                    }
                }
            }

            return adjacencyList;
        }

        private void PrintMap(Dictionary<(int, int), int> map)
        {
            int maxX = map.Keys.Max(k => k.Item1);
            int maxY = map.Keys.Max(k => k.Item2);
            int minX = map.Keys.Min(k => k.Item1);
            int minY = map.Keys.Min(k => k.Item2);

            int spanX = maxX - minX;
            int spanY = maxY - minY;

            List<List<char>> mappedChars = [];

            for (int i = minX; i <= maxX; i++)
            {
                mappedChars.Add([]);

                for (int j = minY; j <= maxY; j++)
                {
                    if (map.TryGetValue((i, j), out int value))
                    {
                        if (value == 0)
                        {
                            mappedChars[i - minX].Add('#');
                        }
                        else if (value == 1)
                        {
                            mappedChars[i - minX].Add('.');
                        }
                        else
                        {
                            mappedChars[i - minX].Add('X');
                        }
                    }
                    else
                    {
                        mappedChars[i - minX].Add('#');
                    }
                }
            }

            for (int i = minX; i <= maxX; i++)
            {
                Console.WriteLine(new string(mappedChars[i - minX].ToArray()));
            }
        }

        private Dictionary<(int, int), int> CreateMap(long[] program)
        {
            this.computer = new(program);

            Queue<(int, int)> queue = [];
            Stack<(int, int)> prevPositions = [];
            Dictionary<(int, int), int> map = [];
            List<(int, int)> visitedTiles = [];

            queue.Enqueue((0, 0));
            visitedTiles.Add((0, 0));
            map.Add((0, 0), 1);

            while (queue.Count > 0)
            {
                (int, int) position = queue.Dequeue();

                List<(int, int, int)> availableNeighbours = GetAvailableNeighbours(position, visitedTiles);

                if (availableNeighbours.Count == 0)
                {
                    if (prevPositions.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        (int, int) prevPosition = prevPositions.Pop();
                        ReturnRobot(position, prevPosition);
                        queue.Enqueue(prevPosition);
                    }
                }
                else
                {
                    (int, int, int) neighbour = availableNeighbours.First();

                    int result = MoveRobot(neighbour.Item1);

                    visitedTiles.Add((neighbour.Item2, neighbour.Item3));
                    map.Add((neighbour.Item2, neighbour.Item3), result);

                    // Robot moved
                    if (result != 0)
                    {
                        queue.Enqueue((neighbour.Item2, neighbour.Item3));
                        prevPositions.Push((position.Item1, position.Item2));
                    }
                    // Wall
                    else
                    {
                        queue.Enqueue((position.Item1, position.Item2));
                    }
                }
            }

            return map;
        }

        private int MoveRobot(int mov)
        {
            if (this.computer == null) throw new Exception("Computer not initialized");

            this.computer.SetInputs([mov]);
            int result = (int)computer.Run(true);
            if (result != 0 && result != 1 && result != 2)
            {
                throw new Exception("Invalid return.");
            }
            return result;
        }

        private void ReturnRobot((int, int) currentPosition, (int, int) previousPosition)
        {
            (int, int) movement = (previousPosition.Item1 - currentPosition.Item1,
                previousPosition.Item2 - currentPosition.Item2);

            int movIndex = this.possibleMovs.IndexOf(movement) + 1;

            if (movIndex <= 0 || movIndex > 4)
            {
                throw new Exception("Invalid movement");
            }

            int result = MoveRobot(movIndex);

            if (result != 1 && result != 2)
            {
                throw new Exception("Invalid return.");
            }
        }

        private List<(int, int, int)> GetAvailableNeighbours((int, int) currentPosition, List<(int, int)> visitedTiles)
        {
            List<(int, int, int)> availableNeighbours = [];

            for (int i = 0; i < this.possibleMovs.Count; i++)
            {
                (int, int) newPosition = currentPosition.Add(this.possibleMovs[i]);
                if (!visitedTiles.Contains(newPosition))
                {
                    availableNeighbours.Add((i + 1, newPosition.Item1, newPosition.Item2));
                }
            }

            return availableNeighbours;
        }
    }
}