namespace Day08;

public class HauntedWasteland
{
    private List<char> mapInstructions;
    private Dictionary<string, (string, string)> mapPuzzle;
    public HauntedWasteland(string[] inputMapLines)
    {
        mapInstructions = new List<char>();
        mapPuzzle = new Dictionary<string, (string, string)>();

        LoadData(inputMapLines);
    }
    
    public int GetStepsToEscape(string startOn = "AAA", bool checkOnlyLastLetter = false)
    {
        var stepsDone = 0;
        var mapPuzzleIndex = startOn;

        if (!mapPuzzle.ContainsKey(startOn))
        {
            return 0;
        }

        var currentIndex = 0;
        
        while (currentIndex <= mapInstructions.Count)
        {
            var mapPuzzleLine = mapPuzzle[mapPuzzleIndex];
            var currentInstruction = mapInstructions[currentIndex];

            var puzzleLine = currentInstruction.Equals('L') ? mapPuzzleLine.Item1 : mapPuzzleLine.Item2;

            stepsDone++;
            currentIndex++;

            if (!checkOnlyLastLetter && puzzleLine.Equals("ZZZ") ||
                checkOnlyLastLetter && puzzleLine.ToCharArray().LastOrDefault().Equals('Z'))
            {
                break;
            }
            
            if (currentIndex > mapInstructions.Count - 1)
            {
                currentIndex = 0;
            }

            mapPuzzleIndex = puzzleLine;
        }

        return stepsDone;
    }

    /// <summary>
    /// Gets the steps to escape using the LCM (Least Common Multiple).
    /// </summary>
    /// <returns>Number of steps</returns>
    public long GetStepsToEscapePart02Lcm()
    {
        var startingNodes = GetMapPuzzleLinesByEndingLetter('A');

        var allSteps = new List<long>();
        Parallel.ForEach(startingNodes, (nodes, loopState) =>
        {
            var threadId = Environment.CurrentManagedThreadId;
            Console.WriteLine($"Thread {threadId} with node {nodes.Key} started to get steps...");
            
            var steps = GetStepsToEscape(nodes.Key, true);
            Console.WriteLine($"Thread {threadId} with node {nodes.Key} finished with {steps} steps.");

            lock (allSteps)
            {
                allSteps.Add(steps);
            }
            
        });

        return MathHelper.CalculateLcm(allSteps.ToArray());
    }

    /// <summary>
    /// Gets the steps to escape using the brute force.
    /// </summary>
    /// <returns>Number of steps</returns>
    public int GetStepsToEscapePart02()
    {
        var stepsDone = 0;
        var startingNodes = GetMapPuzzleLinesByEndingLetter('A');
        var endingNodes = GetMapPuzzleLinesByEndingLetter('Z');

        var mapPuzzleIndex = new List<string>();
        
        mapPuzzleIndex.AddRange(startingNodes.Keys.ToArray());

        var currentIndex = 0;
        
        while (currentIndex <= mapInstructions.Count)
        {
            var currentInstruction = mapInstructions[currentIndex];

            var puzzleLine = new List<string>();
            var allSteps = mapPuzzleIndex
                .Select(puzzleIndex => DoStep(puzzleIndex, currentInstruction, endingNodes))
                .ToList();

            var isNextStepTheLast = allSteps.All(step => step.Item1);

            stepsDone++;
            currentIndex++;

            if (isNextStepTheLast)
            {
                break;
            }
            
            if (currentIndex > mapInstructions.Count - 1)
            {
                currentIndex = 0;
            }

            puzzleLine.Clear();
            puzzleLine.AddRange(allSteps.Select(step => step.Item2));

            mapPuzzleIndex = puzzleLine;
        }

        return stepsDone;
    }

    private (bool, string) DoStep(string mapPuzzleIndex,char currentInstruction, Dictionary<string, (string, string)> endingNodes)
    {
        var mapPuzzleLine = mapPuzzle[mapPuzzleIndex];
        
        var puzzleLine = currentInstruction.Equals('L') ? mapPuzzleLine.Item1 : mapPuzzleLine.Item2;
        
        if (endingNodes.Keys.Any(x => x.Equals(puzzleLine)))
        {
            return (true, puzzleLine);
        }
        
        return (false, puzzleLine);
    }

    private Dictionary<string,(string, string)> GetMapPuzzleLinesByEndingLetter(char endLetterFilter)
    {
        return mapPuzzle
            .Where(mapLine => mapLine.Key.ToCharArray()[2].Equals(endLetterFilter))
            .ToDictionary(mapLine => mapLine.Key, mapLine => mapLine.Value);
    }

    private void LoadData(string[] inputMapLines)
    {
        mapInstructions = inputMapLines[0].ToList();

        for (var i = 0; i < inputMapLines.Length; i++)
        {
            var line = inputMapLines[i];
            if (line.Trim().Length < 1 || i == 0)
            {
                continue;
            }

            var parsedMapLine = GetOneMapLine(line);
            mapPuzzle.Add(parsedMapLine.Item1, (parsedMapLine.Item2, parsedMapLine.Item3));
        }
    }

    private (string, string, string) GetOneMapLine(string mapLine)
    {
        var splitMapLine = mapLine.Split('=');
        var mapLineIndex = splitMapLine[0].Trim().ToUpper();

        var splitDirections = splitMapLine[1].Replace("(", string.Empty).Replace(")", string.Empty).Split(',');
        var directionLeft = splitDirections[0].Trim().ToUpper();
        var directionRight = splitDirections[1].Trim().ToUpper();

        return (mapLineIndex, directionLeft, directionRight);
    }
}