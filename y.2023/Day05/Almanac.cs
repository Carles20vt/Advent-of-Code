namespace Day05;

public class Almanac
{
    private readonly string[] almanacLines;
    private static HashSet<SeedMap> seedMap;
    
    public Almanac(string[] almanacLines)
    {
        this.almanacLines = almanacLines;
        seedMap = new HashSet<SeedMap>();
    }

    public long GetLowestLocation()
    {
        var seedGroups = GetSeedGroup();

        foreach (var seedGroup in seedGroups)
        {
            var seedsList = GetSeedsList(seedGroup);
            BuildSeedMapData(seedsList);
        }
        
        foreach (var map in seedMap)
        {
            Console.WriteLine($"Seed {map.Seed}, soil {map.Soil}, fertilizer {map.Fertilizer}, water {map.Water}, light {map.Light}, temperature {map.Temperature}, humidity {map.Humidity}, location {map.Location}.");
        }

        var lowestLocationNumber = seedMap.Min(x => x.Location);
        return lowestLocationNumber;
    }

    private HashSet<(long, long)> GetSeedGroup()
    {
        var seedString = almanacLines.FirstOrDefault(x => x.Contains("seeds:"));
        var seedsListRaw = GetNumbersFromLine(seedString.Split(':')[1]);
        var seedsList = new HashSet<(long, long)>();

        for (var seedIndex = 0; seedIndex < seedsListRaw.Count; seedIndex = seedIndex + 2)
        {
            seedsList.Add((seedsListRaw[seedIndex], seedsListRaw[seedIndex + 1]));
        }

        return seedsList;
    }
    
    private HashSet<long> GetSeedsList((long, long) seedGroup)
    {
        var seedsList = new HashSet<long>();
        
        for (var rangeIndex = 0; rangeIndex < seedGroup.Item2; rangeIndex++)
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
        
        foreach (var seed in seedsList)
        {
            var soil = GetDestinationByFilter(seed, seedToSoilMapLines);
            var fertilizer = GetDestinationByFilter(soil, soilToFertilizerMapLines);
            var water = GetDestinationByFilter(fertilizer, fertilizerToWaterMapLines);
            var light = GetDestinationByFilter(water, waterToLightMapLines);
            var temperature = GetDestinationByFilter(light, lightToTemperatureMapLines);
            var humidity = GetDestinationByFilter(temperature, temperatureToHumidityMapLines);
            var location = GetDestinationByFilter(humidity, humidityToLocationMapLines);
            
            seedMap.Add(new SeedMap
            {
                Seed = seed,
                Soil = soil,
                Fertilizer = fertilizer,
                Water = water,
                Light = light,
                Temperature = temperature,
                Humidity = humidity,
                Location = location
            });
        }
    }
    
    private long GetDestinationByFilter(long filter, HashSet<string> mapLines)
    {
        foreach (var mapLine in mapLines)
        {
            var splitMapLines = GetNumbersFromLine(mapLine).ToArray();
            var numberFound = GetMatchingNumberOnMap(filter, splitMapLines);

            if (numberFound != -1)
            {
                return numberFound;
            }
        }

        return filter;
    }
    
    private long GetMatchingNumberOnMap(long numberToFind, long[] splitMapLines)
    {
        var destinationRangeStart = splitMapLines[0];
        var sourceRangeStart = splitMapLines[1];
        var rangeLength = splitMapLines[2];
        
        for (var i = 0; i < rangeLength; i++)
        {
            if (numberToFind == sourceRangeStart + i)
            {
                return destinationRangeStart + i;
            }
        }
        
        return -1;
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