namespace Day11;

public class CosmicExpansion
{
    private const char GalaxyToken = '#';
    
    private List<(long, long, long)> foundGalaxies;
    
    public CosmicExpansion(List<string> inputLines, int expansionDistance = 1)
    {
        foundGalaxies = new List<(long, long, long)>();

        LoadData(inputLines, expansionDistance);
    }

    public long GetShortestPathBetweenGalaxies()
    {
        var galaxyPairs = GetGalaxyPairs();
        Console.WriteLine($"\nThere is {galaxyPairs.Count} pairs of galaxies.");

        var sum = galaxyPairs.Sum(x => x.Item3);
        return sum;
    }

    private List<(long, long, long)> GetGalaxyPairs()
    {
        var galaxyPairs = new List<(long, long, long)>();

        Parallel.ForEach(foundGalaxies, (currentGalaxy) =>
        {
            var galaxyPairsNotCurrent = foundGalaxies.Where(x => !x.Equals(currentGalaxy));
            foreach (var galaxy in galaxyPairsNotCurrent)
            {
                var distance = DistanceBetweenTwoGalaxies(currentGalaxy, galaxy);
                lock (galaxyPairs)
                {
                    var isRepeteaded =
                        galaxyPairs.Any(x => x.Item2 == currentGalaxy.Item1 && x.Item1 == galaxy.Item1);
                    if (isRepeteaded)
                    {
                        continue;
                    }
                    
                    galaxyPairs.Add((currentGalaxy.Item1, galaxy.Item1, distance));
                }
            }
        });
        
        return galaxyPairs;
    }

    private void LoadData(List<string> inputLines, int expansionDistance)
    {
        var expansionsOnX = FindExpansionsOnX(inputLines);
        
        var numberOfGalaxies = 0;
        var numberOfExpansionsOnY = 0;
        
        var reversedInputLines = new List<string>(inputLines);
        reversedInputLines.Reverse();
        
        
        for (var y = 0; y < reversedInputLines.Count; y++)
        {
            var line = reversedInputLines[y];

            var isExpansion = line.Replace(".", string.Empty).Trim().Length <= 0;
            if (isExpansion)
            {
                numberOfExpansionsOnY++;
            }

            for (var x = 0; x < line.ToCharArray().Length; x++)
            {
                var value = line[x];
                if (!value.Equals(GalaxyToken))
                {
                    continue;
                }
                
                numberOfGalaxies++;
                    
                foundGalaxies.Add((
                    numberOfGalaxies, 
                    x + (GetExpansionsAmountForX(expansionsOnX, x) * expansionDistance), 
                    y + (numberOfExpansionsOnY * expansionDistance)));
            }
        }
    }

    private static long GetExpansionsAmountForX(List<long> expansionsOnX, long currentX)
    {
        var amount = expansionsOnX.Count(x => x < currentX);
        return amount;
    }

    private static List<long> FindExpansionsOnX(List<string> inputLines)
    {
        var foundExpansions = new List<long>();

        var columnsAmount = inputLines[0].Length;
        var filesAmount = inputLines.Count;

        for (var column = 0; column < columnsAmount; column++)
        {
            var isEmpty = true;
            
            for (var file = 0; file < filesAmount; file++)
            {
                var value = inputLines[file].ToCharArray()[column];
                if (!value.Equals(GalaxyToken))
                {
                    continue;
                }
                
                isEmpty = false;
                break;
            }

            if (isEmpty)
            {
                foundExpansions.Add(column);
            }
        }

        return foundExpansions;
    }

    /// <summary>
    /// Gets the distance between two points using Manhattan.
    /// </summary>
    /// <param name="galaxyA">Point A</param>
    /// <param name="galaxyB">Point B</param>
    /// <returns>Distance</returns>
    private static long DistanceBetweenTwoGalaxies((long, long, long) galaxyA, (long, long, long) galaxyB)
    {
        var distance = Math.Abs(galaxyA.Item2 - galaxyB.Item2) + Math.Abs(galaxyA.Item3 - galaxyB.Item3);
        
        return distance;
    }
}