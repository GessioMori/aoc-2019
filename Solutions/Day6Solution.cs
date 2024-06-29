using Main.Tools;

namespace Main.Solutions
{
    public class Day6Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            List<Planet> planets = ListPlanets(inputData);

            int orbitsSum = 0;

            foreach (Planet planet in planets)
            {
                Planet currentPlanet = planet;

                while (currentPlanet != null && currentPlanet.Name != "COM")
                {
                    orbitsSum++;
                    if (currentPlanet.Orbits != null)
                    {
                        currentPlanet = currentPlanet.Orbits;
                    }
                }
            }

            return orbitsSum.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            string[] invalidPlanets = ["COM", "SAN", "YOU"];

            List<Planet> planets = ListPlanets(inputData);

            List<string> visitedPlanets = [];
            Queue<string> queue = new();

            Dictionary<string, List<string>> adjacencyDict = [];
            Dictionary<string, int> distances = [];

            foreach (Planet planet in planets)
            {
                if (!invalidPlanets.Contains(planet.Name))
                {
                    distances.Add(planet.Name, 0);

                    if (!adjacencyDict.ContainsKey(planet.Name))
                    {
                        adjacencyDict.Add(planet.Name, []);
                    }
                    if (!adjacencyDict.ContainsKey(planet.OrbitsName))
                    {
                        adjacencyDict.Add(planet.OrbitsName, []);
                    }

                    adjacencyDict[planet.Name].Add(planet.OrbitsName);
                    adjacencyDict[planet.OrbitsName].Add(planet.Name);
                }
            }

            string planetYouOrbit = planets.First(p => p.Name == "YOU").OrbitsName!;
            string planetSantaOrbits = planets.First(p => p.Name == "SAN").OrbitsName!;

            visitedPlanets.Add(planetYouOrbit);
            queue.Enqueue(planetYouOrbit);

            while (queue.Count > 0)
            {
                string currentPlanet = queue.Dequeue();

                foreach (string nextPlanet in adjacencyDict[currentPlanet])
                {
                    if (!visitedPlanets.Contains(nextPlanet))
                    {
                        visitedPlanets.Add(nextPlanet);
                        distances[nextPlanet] = distances[currentPlanet] + 1;
                        queue.Enqueue(nextPlanet);

                        if (nextPlanet == planetSantaOrbits)
                        {
                            return distances[nextPlanet].ToString();
                        }
                    }
                }
            }

            throw new Exception("Result not found.");
        }

        private List<Planet> ListPlanets(string[] input)
        {
            List<Planet> planets = new List<Planet>();

            foreach (string s in input)
            {
                string[] line = s.Split(')');
                planets.Add(new Planet { Name = line[1], OrbitsName = line[0] });
            }

            planets.Add(new Planet { Name = "COM" });

            foreach (Planet planet in planets)
            {
                if (planet.Name != "COM")
                {
                    Planet centerPlanet = planets.First(p => p.Name == planet.OrbitsName);
                    planet.Orbits = centerPlanet;
                }
            }

            return planets;
        }

    }

    record Planet
    {
        public string Name = "";
        public string OrbitsName = "";
        public Planet? Orbits;
    }
}