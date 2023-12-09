namespace Day05;

public class Almanac
{
    private readonly string[] _almanacLines;
    private List<(long, long)> seedGroups;
    private Dictionary<string, List<(long, long, long, long)>> mapData;

    public Almanac(string[] almanacLines)
    {
        _almanacLines = almanacLines;
        seedGroups = GetSeedGroup();
        mapData = new Dictionary<string, List<(long, long, long, long)>>();

        LoadMaps();
    }

    private void LoadMaps()
    {
        var seedToSoilMapLines = GetMapLinesByMapName(_almanacLines, "seed-to-soil map:");
        var seedToSoilMapData = GetMapDataFromLine(seedToSoilMapLines);
        mapData.Add("SeedToSoil", seedToSoilMapData);
        
        var soilToFertilizerMapLines = GetMapLinesByMapName(_almanacLines, "soil-to-fertilizer map:");
        var soilToFertilizerMapData = GetMapDataFromLine(soilToFertilizerMapLines);
        mapData.Add("SoilToFertilizer", soilToFertilizerMapData);
        
        var fertilizerToWaterMapLines = GetMapLinesByMapName(_almanacLines, "fertilizer-to-water map:");
        var fertilizerToWaterMapData = GetMapDataFromLine(fertilizerToWaterMapLines);
        mapData.Add("FertilizerToWater", fertilizerToWaterMapData);
        
        var waterToLightMapLines = GetMapLinesByMapName(_almanacLines, "water-to-light map:");
        var waterToLightMapData = GetMapDataFromLine(waterToLightMapLines);
        mapData.Add("WaterToLight", waterToLightMapData);
        
        var lightToTemperatureMapLines = GetMapLinesByMapName(_almanacLines, "light-to-temperature map:");
        var lightToTemperatureMapData = GetMapDataFromLine(lightToTemperatureMapLines);
        mapData.Add("LightToTemperature", lightToTemperatureMapData);
        
        var temperatureToHumidityMapLines = GetMapLinesByMapName(_almanacLines, "temperature-to-humidity map:");
        var temperatureToHumidityMapData = GetMapDataFromLine(temperatureToHumidityMapLines);
        mapData.Add("TemperatureToHumidity", temperatureToHumidityMapData);
        
        var humidityToLocationMapLines = GetMapLinesByMapName(_almanacLines, "humidity-to-location map:");
        var humidityToLocationMapData = GetMapDataFromLine(humidityToLocationMapLines);
        mapData.Add("HumidityToLocation", humidityToLocationMapData);
     }
    
    private static List<(long, long, long, long)> GetMapDataFromLine(List<string> seedStrings)
    {
        var seedList = new List<(long, long, long, long)>();

        foreach (var seedString in seedStrings)
        {
            var splitSeeds = seedString.Split(' ');

            var destinationRangeStart = splitSeeds[0];
            var sourceRangeStart = splitSeeds[1];
            var rangeLength = splitSeeds[2];

            var parsedDestinationRangeStart = long.Parse(destinationRangeStart);
            var parsedSourceRangeStart = long.Parse(sourceRangeStart);
            var parsedRangeLengthParsed = long.Parse(rangeLength);

            var sourceRangeEnd = parsedSourceRangeStart + parsedRangeLengthParsed;
            var destinationRangeEnd = parsedDestinationRangeStart + parsedRangeLengthParsed;
            
            seedList.Add((parsedDestinationRangeStart, destinationRangeEnd, parsedSourceRangeStart, sourceRangeEnd));
        }

        return seedList.OrderByDescending(x => x.Item1).ToList();
    }

    public long GetLowestLocation()
    {
        long lowestSeed = -1;

        Console.WriteLine($"Loaded {seedGroups.Count} Seed Groups.");

        const int maxLocationToCheck = 50000000;
        const int partitions = maxLocationToCheck / 10;
        
        var groupedLocationsToCheck = new List<(long, long)>();

        for (var i = 0; i < maxLocationToCheck; i += partitions)
        {
            groupedLocationsToCheck.Add((i, i + partitions));
        }
        
        Parallel.ForEach(groupedLocationsToCheck, (groupedLocationToCheck, loopState) =>
        {
            var threadId = Environment.CurrentManagedThreadId;

            Console.WriteLine($"The thread {threadId} started the search...");

            var result = CheckSeedGroups(groupedLocationToCheck.Item1, groupedLocationToCheck.Item2);

            if (result == -1)
            {
                Console.WriteLine($"The thread {threadId} not found any lowest location.");
                return;
            }

            Console.WriteLine($"The thread {threadId} found lowest location at: {result}.");

            lock (typeof(Program))
            {
                if (lowestSeed == -1 || lowestSeed > result)
                {
                    lowestSeed = result;
                }
            }
        });

        return lowestSeed;
    }

    private long CheckSeedGroups(long startIndex, long endIndex)
    {
        var locationFound = endIndex + 1;

        for (var location = startIndex; location <= endIndex; location++)
        {
            var humidity = GetDestinationByFilter(location, "HumidityToLocation");
            var temperature = GetDestinationByFilter(humidity, "TemperatureToHumidity");
            var light = GetDestinationByFilter(temperature, "LightToTemperature");
            var water = GetDestinationByFilter(light, "WaterToLight");
            var fertilizer = GetDestinationByFilter(water, "FertilizerToWater");
            var soil = GetDestinationByFilter(fertilizer, "SoilToFertilizer");
            var seed = GetDestinationByFilter(soil, "SeedToSoil", true);

            if (!IsInAnySeedGroup(seed))
            {
                continue;
            }

            Console.WriteLine($"[Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}.");

            locationFound = location;

            break;
        }

        if (locationFound == endIndex + 1)
        {
            return -1;
        }

        return locationFound;
    }

    private bool IsInAnySeedGroup(long seedToCheck)
    {
        return seedGroups.Any(seedGroup => seedGroup.Item1 <= seedToCheck && seedGroup.Item2 >= seedToCheck);
    }

    private long GetDestinationByFilter(long destination, string mapName, bool mustExist = false)
    {
        var mapRanges = mapData[mapName];

        foreach (var range in mapRanges)
        {
            var numberFound = GetMatchingNumberOnMap(destination, range);

            if (numberFound != -1)
            {
                return numberFound;
            }
        }

        return mustExist ? -1 : destination;
    }

    private static long GetMatchingNumberOnMap(long destinationToFind, (long, long, long, long) range)
    {
        var destinationRangeStart = range.Item1;
        var destinationRangeEnd = range.Item2;
        var sourceRangeStart = range.Item3;
        var sourceRangeEnd = range.Item4;

        if (destinationRangeStart > destinationToFind || destinationToFind > destinationRangeEnd)
        {
            return -1;
        }
        
        var sourceNumber = (destinationToFind - destinationRangeStart) + sourceRangeStart;
        return sourceNumber;
    }
    
    private List<(long, long)> GetSeedGroup()
    {
        var seedString = _almanacLines.FirstOrDefault(x => x.Contains("seeds:"));
        var seedsListRaw = GetNumbersFromLine(seedString.Split(':')[1]);
        var seedsList = new List<(long, long)>();

        for (var seedIndex = 0; seedIndex < seedsListRaw.Count; seedIndex = seedIndex + 2)
        {
            seedsList.Add((seedsListRaw[seedIndex], seedsListRaw[seedIndex] + seedsListRaw[seedIndex + 1]));
        }

        return seedsList;
    }
    
    private static List<long> GetNumbersFromLine(string seedString)
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
    
    private static List<string> GetMapLinesByMapName(string[] almanacLines, string mapName)
    {
        var indexOfSeedToSoil = Array.IndexOf(almanacLines, mapName);
        var mapLines = new List<string>();

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