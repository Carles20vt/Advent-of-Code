namespace Day05;

public class Almanac
{
    private readonly string[] almanacLines;
    private long lowestLocationNumber;
    
    public Almanac(string[] almanacLines)
    {
        this.almanacLines = almanacLines;
    }

    public long GetLowestLocation()
    {
        var seedGroups = GetSeedGroup();
        
        /*
        Console.WriteLine($"Loaded {seedGroups.Count} seed groups.");

        ClearOverlappingRanges(seedGroups);
        
        Console.WriteLine($"Loaded {seedGroups.Count} seed groups after remove overlapping ranges.");
        */

        foreach (var seedGroup in seedGroups)
        {
            var seedsList = GetSeedsList(seedGroup);
            BuildSeedMapData(seedsList);
        }
        
        return this.lowestLocationNumber;
    }
    
    private static void ClearOverlappingRanges(HashSet<(long, long)> seedGroups)
    {
        var seedGroupsWithoutOverlapping = new HashSet<(long, long)>(seedGroups);

        foreach (var rangeA in seedGroups)
        {
            foreach (var rangeB in seedGroups.Where(rangeB => rangeA != rangeB && IsOverlapping(rangeA, rangeB)))
            {
                seedGroupsWithoutOverlapping.Remove(rangeB);
            }
        }

        seedGroups.Clear();
        seedGroups.UnionWith(seedGroupsWithoutOverlapping);
    }
    
    private static bool IsOverlapping((long, long) rangeA, (long, long) rangeB)
    {
        return rangeA.Item1 < rangeB.Item1 && rangeA.Item2 > rangeB.Item2;
    }

    private HashSet<(long, long)> GetSeedGroup()
    {
        var seedString = almanacLines.FirstOrDefault(x => x.Contains("seeds:"));
        var seedsListRaw = GetNumbersFromLine(seedString.Split(':')[1]);
        var seedsList = new HashSet<(long, long)>();
        
        for (var seedIndex = 0; seedIndex < seedsListRaw.Count; seedIndex = seedIndex + 2)
        {
            seedsList.Add((seedsListRaw[seedIndex], seedsListRaw[seedIndex] + seedsListRaw[seedIndex + 1]));
        }

        return seedsList;
    }
    
    private List<long> GetNumbersFromLine(string seedString)
    {
        var seedList = new List<long>();
        var splitSeeds = seedString.Split(' ');
        
        foreach (var seed in splitSeeds)
        {
            var isParsed = long.TryParse(seed, out var parsedSeed);
            if (isParsed)
            {
                seedList.Add(parsedSeed); 
            }
        }
        
        return seedList;
    }
    
    private HashSet<long> GetSeedsList((long, long) seedGroup)
    {
        var seedsList = new HashSet<long>();

        var repetitions = seedGroup.Item2 - seedGroup.Item1;
        
        for (var rangeIndex = 0; rangeIndex < repetitions; rangeIndex++)
        {
            seedsList.Add(seedGroup.Item1 + rangeIndex);
        }

        return seedsList;
    }

    private void BuildSeedMapData(HashSet<long> seedsList)
    {
        var seedToSoilMapLines = GetMapLinesByMapName(almanacLines, "seed-to-soil map:");
        var soilToFertilizerMapLines = GetMapLinesByMapName(almanacLines, "soil-to-fertilizer map:");
        var fertilizerToWaterMapLines = GetMapLinesByMapName(almanacLines, "fertilizer-to-water map:");
        var waterToLightMapLines = GetMapLinesByMapName(almanacLines, "water-to-light map:");
        var lightToTemperatureMapLines = GetMapLinesByMapName(almanacLines, "light-to-temperature map:");
        var temperatureToHumidityMapLines = GetMapLinesByMapName(almanacLines, "temperature-to-humidity map:");
        var humidityToLocationMapLines = GetMapLinesByMapName(almanacLines, "humidity-to-location map:");
        
        Console.WriteLine($"Seeds to process: {seedsList.Count}");
        
        foreach (var seed in seedsList)
        {
            var soil = GetDestinationByFilter(seed, seedToSoilMapLines);
            var fertilizer = GetDestinationByFilter(soil, soilToFertilizerMapLines);
            var water = GetDestinationByFilter(fertilizer, fertilizerToWaterMapLines);
            var light = GetDestinationByFilter(water, waterToLightMapLines);
            var temperature = GetDestinationByFilter(light, lightToTemperatureMapLines);
            var humidity = GetDestinationByFilter(temperature, temperatureToHumidityMapLines);
            var location = GetDestinationByFilter(humidity, humidityToLocationMapLines);

            if (lowestLocationNumber > location || 
                lowestLocationNumber == 0)
            {
                lowestLocationNumber = location;
                
                Console.WriteLine($"Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}.");
            }
        }
    }
    
    private long GetDestinationByFilter(long filter, HashSet<string> mapLines)
    {
        var mapData = GetMapDataFromLine(mapLines);
        
        //ClearOverlappingRanges(mapData);
        
        foreach (var data in mapData)
        {
            var numberFound = GetMatchingNumberOnMap(filter, data);

            if (numberFound != -1)
            {
                return numberFound;
            }
        }

        return filter;
    }
    
    private static void ClearOverlappingRanges(HashSet<(long, long, long)> seedGroups)
    {
        var seedGroupsWithoutOverlapping = new HashSet<(long, long, long)>(seedGroups);

        foreach (var rangeA in seedGroups)
        {
            foreach (var rangeB in seedGroups.Where(rangeB => rangeA != rangeB && IsOverlapping(rangeA, rangeB)))
            {
                seedGroupsWithoutOverlapping.Remove(rangeB);
            }
        }

        seedGroups.Clear();
        seedGroups.UnionWith(seedGroupsWithoutOverlapping);
    }
    
    private static bool IsOverlapping((long, long, long) rangeA, (long, long, long) rangeB)
    {
        return rangeA.Item2 < rangeB.Item2 && rangeA.Item3 > rangeB.Item3;
    }
    
    private HashSet<(long, long, long)> GetMapDataFromLine(HashSet<string> seedStrings)
    {
        var seedList = new HashSet<(long, long, long)>();

        foreach (var seedString in seedStrings)
        {
            var splitSeeds = seedString.Split(' ');

            var destinationRangeStart = splitSeeds[0];
            var sourceRangeStart = splitSeeds[1];
            var rangeLength = splitSeeds[2];
            
            var isDestinationRangeStartParsed = long.TryParse(destinationRangeStart, out var parsedDestinationRangeStart);
            var isSourceRangeStartParsed = long.TryParse(sourceRangeStart, out var parsedSourceRangeStart);
            var isRangeLengthParsed = long.TryParse(rangeLength, out var parsedRangeLengthParsed);
        
            var sourceRangeEnd= parsedSourceRangeStart + parsedRangeLengthParsed; 
        
            if (isDestinationRangeStartParsed &&
                isSourceRangeStartParsed &&
                isRangeLengthParsed)
            {
                seedList.Add((parsedDestinationRangeStart, parsedSourceRangeStart, sourceRangeEnd));
            }
        }

        return seedList;
    }
    
    private long GetMatchingNumberOnMap(long numberToFind, (long, long, long) splitMapLines)
    {
        var destinationRangeStart = splitMapLines.Item1;
        var sourceRangeStart = splitMapLines.Item2;
        var sourceRangeEnd = splitMapLines.Item3;

        if (sourceRangeStart > numberToFind ||
            sourceRangeEnd < numberToFind)
        {
            return -1;
        }

        var matchingNumber = numberToFind - sourceRangeStart + destinationRangeStart;
        return matchingNumber;
    }
    
    private static HashSet<string> GetMapLinesByMapName(string[] almanacLines, string mapName)
    {
        var indexOfSeedToSoil = Array.IndexOf(almanacLines, mapName);
        var mapLines = new HashSet<string>();

        for (var i = indexOfSeedToSoil + 1; i < almanacLines.Length; i++)
        {
            var currentLine = almanacLines[i];
            var isEmpty = currentLine.Trim().Equals(string.Empty);
            if (isEmpty)
            {
                break;
            }

            mapLines.Add(currentLine);
        }

        return mapLines;
    }
}