using Main.Tools;
using System.Text.RegularExpressions;

namespace Main.Solutions
{
    public class Day14Solution : ISolution
    {
        private Dictionary<string, long> stock = [];
        private List<Reaction> reactions = [];

        public string RunPartA(string[] inputData)
        {
            this.reactions = ParseReactions(inputData);

            return GetOreRequired(1).ToString();
        }

        public string RunPartB(string[] inputData)
        {
            this.reactions = ParseReactions(inputData);

            long availableOre = 1000000000000;

            long oreConsumedByOneFuel = GetOreRequired(1);

            long amountOfFuelProduced = availableOre / oreConsumedByOneFuel;

            while (GetOreRequired(++amountOfFuelProduced) <= availableOre)
            {

            }

            amountOfFuelProduced--;

            return amountOfFuelProduced.ToString();
        }

        private long GetOreRequired(long fuelAmount)
        {
            long initialOre = 10000000000000;
            this.stock.Clear();
            this.stock.Add("ORE", initialOre);

            CreateElement("FUEL", fuelAmount);

            return initialOre - this.stock["ORE"];
        }

        private void CreateElement(string productName, long productAmountNeeded)
        {
            this.stock.TryAdd(productName, 0);

            if (this.stock[productName] < productAmountNeeded)
            {
                Reaction? reaction = this.reactions.FirstOrDefault(r => r.Product.Name == productName);

                if (reaction != null)
                {
                    long timesNeeded = (long)Math.Ceiling((double)productAmountNeeded / reaction.Product.Amount);

                    foreach (Element ingredient in reaction.Ingredients)
                    {
                        this.stock.TryAdd(ingredient.Name, 0);

                        this.stock[ingredient.Name] -= timesNeeded * ingredient.Amount;

                        if (this.stock[ingredient.Name] < 0)
                        {
                            CreateElement(ingredient.Name, -this.stock[ingredient.Name]);
                        }
                    }

                    this.stock[productName] += timesNeeded * reaction.Product.Amount;
                }
                else
                {
                    throw new Exception("No reaction found.");
                }
            }
            else
            {
                this.stock[productName] -= productAmountNeeded;
            }
        }

        private List<Reaction> ParseReactions(string[] input)
        {
            List<Reaction> reactions = [];

            foreach (string line in input)
            {
                MatchCollection matches = Regex.Matches(line, @"(\d+) (\w+)");

                List<Element> elements = [];

                foreach (Match match in matches.Cast<Match>().SkipLast(1))
                {
                    elements.Add(new Element(match.Groups[2].Value,
                        int.Parse(match.Groups[1].Value)));
                }

                Element product = new(matches[^1].Groups[2].Value,
                    int.Parse(matches[^1].Groups[1].Value));

                reactions.Add(new Reaction(product, elements));
            }

            return reactions;
        }
    }

    internal class Reaction(Element product, List<Element> ingredients)
    {
        public Element Product = product;
        public List<Element> Ingredients = ingredients;
    }

    internal class Element(string name, int amount)
    {
        public string Name = name;
        public int Amount = amount;
    }
}