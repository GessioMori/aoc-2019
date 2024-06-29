using Main.Tools;

namespace Main.Solutions
{
    public class Day8Solution : ISolution
    {
        public string RunPartA(string[] inputData)
        {
            string line = inputData[0];

            int height = 6;
            int width = 25;

            int layerSize = height * width;

            int numOfLayers = line.Length / layerSize;

            int minAmountOfZeros = int.MaxValue;
            int numberOfOnesAndTwosOnSelectedLayer = 0;

            for (int i = 0; i < numOfLayers; i++)
            {
                string layer = line.Substring(i * layerSize, layerSize);
                int amountOfZeros = layer.Count(c => c == '0');

                if (amountOfZeros < minAmountOfZeros)
                {
                    minAmountOfZeros = amountOfZeros;
                    numberOfOnesAndTwosOnSelectedLayer = layer.Count(c => c == '1') * layer.Count(c => c == '2');
                }
            }

            return numberOfOnesAndTwosOnSelectedLayer.ToString();
        }

        public string RunPartB(string[] inputData)
        {
            string line = inputData[0];

            int height = 6;
            int width = 25;

            int layerSize = height * width;

            int numOfLayers = line.Length / layerSize;

            List<string> layers = [];

            for (int i = 0; i < numOfLayers; i++)
            {
                string layer = line.Substring(i * layerSize, layerSize);
                layers.Add(layer);
            }

            string finalLayer = "";

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    foreach (string layer in layers)
                    {
                        char currentColor = layer[i * width + j];
                        if (currentColor != '2')
                        {
                            if (currentColor == '0')
                            {
                                finalLayer += ' ';
                            }
                            else
                            {
                                finalLayer += '#';
                            }
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < height; i++)
            {
                Console.WriteLine(finalLayer.Substring(i * width, width));
            }

            return finalLayer;
        }
    }
}