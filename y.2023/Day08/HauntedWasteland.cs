namespace Day08;

public class HauntedWasteland
{
    private int stepsDone;
    private List<char> mapInstructions;
    private Dictionary<string, (string, string)> mapPuzzle;
    public HauntedWasteland(string[] inputMapLines)
    {
        stepsDone = 0;
        mapInstructions = new List<char>();
        mapPuzzle = new Dictionary<string, (string, string)>();

        LoadData(inputMapLines);
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

    public int GetStepsToSoEscape(string startOn = "AAA")
    {
        var mapPuzzleIndex = startOn;

        var currentIndex = 0;
        
        while (currentIndex <= mapInstructions.Count)
        {
            var mapPuzzleLine = mapPuzzle[mapPuzzleIndex];
            var currentInstruction = mapInstructions[currentIndex];

            var puzzleLine = currentInstruction.Equals('L') ? mapPuzzleLine.Item1 : mapPuzzleLine.Item2;

            stepsDone++;
            currentIndex++;

            if (puzzleLine.Equals("ZZZ"))
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
}