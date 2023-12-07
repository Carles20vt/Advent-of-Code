namespace Day05;

public class Almanac
{
    private readonly string[] _almanacLines;

    public Almanac(string[] almanacLines)
    {
        _almanacLines = almanacLines;
    }

    public long GetLowestLocation()
    {
        long lowestSeed = -1;

        var seedGroups = GetSeedGroup();

        Console.WriteLine($"Loaded {seedGroups.Count} Seed Groups.");

        Parallel.ForEach(seedGroups, (seedGroup, loopState) =>
        {
            var threadId = Environment.CurrentManagedThreadId;
            var group = GetSeedsList(seedGroup);

            Console.WriteLine($"The thread {threadId} started the search...");

            var result = CheckSeedGroups(group);

            Console.WriteLine($"The thread {threadId} found lowest location at: {result}.");

            lock (typeof(Program))
            {
                if (lowestSeed == -1 || lowestSeed > result)
                {
                    lowestSeed = result;
                    
                    //loopState.Break();
                }
            }
        });

        return lowestSeed;
    }

    private long CheckSeedGroups(List<long> seedsList)
    {
        var seedsToCheck = seedsList.ToHashSet();

        var seedToSoilMapLines = GetMapLinesByMapName(_almanacLines, "seed-to-soil map:");
        var soilToFertilizerMapLines = GetMapLinesByMapName(_almanacLines, "soil-to-fertilizer map:");
        var fertilizerToWaterMapLines = GetMapLinesByMapName(_almanacLines, "fertilizer-to-water map:");
        var waterToLightMapLines = GetMapLinesByMapName(_almanacLines, "water-to-light map:");
        var lightToTemperatureMapLines = GetMapLinesByMapName(_almanacLines, "light-to-temperature map:");
        var temperatureToHumidityMapLines = GetMapLinesByMapName(_almanacLines, "temperature-to-humidity map:");
        var humidityToLocationMapLines = GetMapLinesByMapName(_almanacLines, "humidity-to-location map:");

        for (var location = 0; location < 100000000; location++)
        {
            var humidity = GetInverseDestinationByFilter(location, humidityToLocationMapLines);
            var temperature = GetInverseDestinationByFilter(humidity, temperatureToHumidityMapLines);
            var light = GetInverseDestinationByFilter(temperature, lightToTemperatureMapLines);
            var water = GetInverseDestinationByFilter(light, waterToLightMapLines);
            var fertilizer = GetInverseDestinationByFilter(water, fertilizerToWaterMapLines);
            var soil = GetInverseDestinationByFilter(fertilizer, soilToFertilizerMapLines);
            var seed = GetInverseDestinationByFilter(soil, seedToSoilMapLines);

            if (!seedsToCheck.Contains(seed))
            {
                continue;
            }

            Console.WriteLine(
                $"[Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}.");

            return location;
        }

        return -1;
    }

    private long GetInverseDestinationByFilter(long filter, List<string> mapLines)
    {
        var mapData = GetMapDataFromLine(mapLines);

        foreach (var data in mapData)
        {
            var numberFound = GetInverseMatchingNumberOnMap(filter, data);

            if (numberFound != -1)
            {
                return numberFound;
            }
        }

        return filter;
    }

    private List<(long, long, long)> GetMapDataFromLine(List<string> seedStrings)
    {
        var seedList = new List<(long, long, long)>();

        foreach (var seedString in seedStrings)
        {
            var splitSeeds = seedString.Split(' ');

            var destinationRangeStart = splitSeeds[0];
            var sourceRangeStart = splitSeeds[1];
            var rangeLength = splitSeeds[2];

            var isDestinationRangeStartParsed =
                long.TryParse(destinationRangeStart, out var parsedDestinationRangeStart);
            var isSourceRangeStartParsed = long.TryParse(sourceRangeStart, out var parsedSourceRangeStart);
            var isRangeLengthParsed = long.TryParse(rangeLength, out var parsedRangeLengthParsed);

            var sourceRangeEnd = parsedSourceRangeStart + parsedRangeLengthParsed;

            if (isDestinationRangeStartParsed &&
                isSourceRangeStartParsed &&
                isRangeLengthParsed)
            {
                seedList.Add((parsedDestinationRangeStart, parsedSourceRangeStart, sourceRangeEnd));
            }
        }

        return seedList;
    }

    private long GetInverseMatchingNumberOnMap(long numberToFind, (long, long, long) splitMapLines)
    {
        var destinationRangeStart = splitMapLines.Item1;
        var sourceRangeStart = splitMapLines.Item2;
        var sourceRangeEnd = splitMapLines.Item3;


        var matchingNumber = numberToFind + sourceRangeStart - destinationRangeStart;
        if (sourceRangeStart > matchingNumber ||
            sourceRangeEnd < matchingNumber)
        {
            return -1;
        }

        return matchingNumber;
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

    private List<long> GetSeedsList((long, long) seedGroup)
    {
        var seedsList = new List<long>();

        var repetitions = seedGroup.Item2 - seedGroup.Item1;

        for (var rangeIndex = 0; rangeIndex < repetitions; rangeIndex++)
        {
            seedsList.Add(seedGroup.Item1 + rangeIndex);
        }

        return seedsList;
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