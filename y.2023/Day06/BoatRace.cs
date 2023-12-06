namespace Day06;

public static class BoatRace
{
    public static long GetNumberOfWaysPower(string[] raceDocument)
    {
        var timeListString = raceDocument.FirstOrDefault(x => x.Contains("Time:"))?.Split(':')[1];
        var distanceListString = raceDocument.FirstOrDefault(x => x.Contains("Distance:"))?.Split(':')[1];
        
        var timeList = GetNumbersFromLine(timeListString);
        var distanceList = GetNumbersFromLine(distanceListString);

        var numberOfWinnerRacesPower = GetNumberOfWays(timeList, distanceList);

        return numberOfWinnerRacesPower;
    }

    public static long GetNumberOfWaysPowerPart2(string[] raceDocument)
    {
        var timeListString = raceDocument.FirstOrDefault(x => x.Contains("Time:"))?.Split(':')[1];
        var distanceListString = raceDocument.FirstOrDefault(x => x.Contains("Distance:"))?.Split(':')[1];
        
        var timeList = GetNumberFromLineWithKerning(timeListString);
        var distanceList = GetNumberFromLineWithKerning(distanceListString);

        var numberOfWinnerRacesPower = GetNumberOfWays(timeList, distanceList);

        return numberOfWinnerRacesPower;
    }

    private static long GetNumberOfWays(List<long> timeList, List<long> distanceList)
    {
        var numberOfRaces = timeList.Count;
        long numberOfWinnerRacesPower = 0;

        for (var i = 0; i < numberOfRaces; i++)
        {
            var winnerRaces = GetWinnerRaces(timeList[i], distanceList[i]);
            var numberOfWinnerRaces = winnerRaces.Count;

            if (numberOfWinnerRacesPower > 0)
            {
                numberOfWinnerRacesPower *= numberOfWinnerRaces;
            }
            else
            {
                numberOfWinnerRacesPower = numberOfWinnerRaces;
            }


            Console.WriteLine(
                $"Race #{i} Time: {timeList[i]} - Record distance: {distanceList[i]} --> {numberOfWinnerRaces} ways to won.");
        }

        return numberOfWinnerRacesPower;
    }
    
    private static List<long> GetNumbersFromLine(string? numbersString)
    {
        var numbers = new List<long>();
        var splitNumberString = numbersString?.Split(' ');

        if (splitNumberString == null)
        {
            return numbers;
        }
        
        foreach (var numberString in splitNumberString)
        {
            var isParsed = int.TryParse(numberString, out var parsedSeed);
            if (isParsed)
            {
                numbers.Add(parsedSeed);
            }
        }

        return numbers;
    }
    
    private static List<long> GetNumberFromLineWithKerning(string? numbersString)
    {
        var numbers = new List<long>();
        var splitNumberString = numbersString?.Replace(" ", string.Empty).Trim();

        if (splitNumberString == null)
        {
            return numbers;
        }
        
        var isParsed = long.TryParse(splitNumberString, out var parsedSeed);
        if (isParsed)
        {
            numbers.Add(parsedSeed);
        }

        return numbers;
    }

    private static HashSet<long> GetWinnerRaces(long currentRaceTime, long currentRecordDistance)
    {
        var validRaces = new HashSet<long>();
        
        for (var i = 1; i < currentRaceTime - 1; i++)
        {
            var travelledDistance = (currentRaceTime - i) * i;

            if (travelledDistance > currentRecordDistance)
            {
                validRaces.Add(i);
            }
        }
        
        return validRaces;
    }
}