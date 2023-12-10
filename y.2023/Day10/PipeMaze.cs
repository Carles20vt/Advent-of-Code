using System.Security.Cryptography;

namespace Day10;

public class PipeMaze
{
    private const char StarterPoint = 'S';
    private const char InvalidValue = '.';
    
    private readonly List<char> allowedPipesGoUpper;
    private readonly List<char> allowedPipesGoDown;
    private readonly List<char> allowedPipesLeft;
    private readonly List<char> allowedPipesRight;
    
    private readonly char[,] mazeMatrix;
    private (char, int, int) starterPoint;
    private List<(char, int, int)> visitedCell;
    private List<(char, int, int)> longerVisitedCell;

    private long totalSteps;
    
    public PipeMaze(List<string> inputLines)
    {
        allowedPipesGoUpper = new List<char>{'|', 'F', '7', 'S'};
        allowedPipesGoDown = new List<char>{'|', 'L', 'J', 'S'};
        allowedPipesLeft = new List<char>{'-', 'L', 'F', 'S'};
        allowedPipesRight = new List<char>{'-', 'J', '7', 'S'};
        
        var xSize = inputLines[0].Length;
        var ySize = inputLines.Count;
        mazeMatrix = new char[xSize, ySize];

        starterPoint = (StarterPoint, -1, -1);
        visitedCell = new List<(char, int, int)>();
        longerVisitedCell = new List<(char, int, int)>();

        totalSteps = 0;
        
        LoadMaze(inputLines);
    }

    private void LoadMaze(List<string> inputLines)
    {
        var reversedInputLines = new List<string>(inputLines);
        reversedInputLines.Reverse();
        
        for (var y = 0; y < reversedInputLines.Count; y++)
        {
            var line = reversedInputLines[y];

            for (var x = 0; x < line.ToCharArray().Length; x++)
            {
                var value = line[x];
                if (value.Equals(StarterPoint))
                {
                    starterPoint = (value, x, y);
                }
                
                mazeMatrix[x, y] = value;
            }
        }
    }

    public long GetFarthestPoint()
    {
        var currentPathVisitedCells = new List<(char, int, int)>();
        
        DoOneStep(starterPoint, 1, currentPathVisitedCells);

        return totalSteps / 2;
    }

    public int GetEnclosedTiles()
    {
        GetFarthestPoint();
        
        var xMazeSize = mazeMatrix.GetLength(0); 
        var yMazeSize = mazeMatrix.GetLength(1);

        var enclosedTiles = 0;

        for (var y = 0; y < yMazeSize; y++)
        {
            for (var x = 0; x < xMazeSize; x++)
            {
                var currentChar = mazeMatrix[x, y];
                
                if (longerVisitedCell.Contains((currentChar, x, y)))
                {
                    continue;
                }

                var crossedAmount = GetCrossedAmount(x, y);
                if (crossedAmount != 0 && crossedAmount % 2 != 0)
                {
                    Console.WriteLine($"{currentChar} - X{x} - Y{y} - Crossed {crossedAmount}");

                    enclosedTiles++;
                }
            }
        }

        return enclosedTiles;
    }

    private int GetCrossedAmount(int x, int y)
    {
        var mazeSize = mazeMatrix.GetLength(1);
        var crossedAmount = 0;
        
        for (var i = x; i < mazeSize; i++)
        {
            var nextPointToCheck = mazeMatrix[i, y];
            var crossed = longerVisitedCell.Contains((nextPointToCheck, i, y));

            if (crossed)
            {
                crossedAmount++;
            }
        }

        return crossedAmount;
    }

    private void DoOneStep((char, int, int) currentPosition, long steps, List<(char, int, int)> currentPathVisitedCells)
    {
        currentPathVisitedCells.Add(currentPosition);
        visitedCell.Add(currentPosition);
        var nextPositionCandidates = GetNextPositionCandidates(currentPosition);
        
        var commonElements = nextPositionCandidates.Intersect(visitedCell).ToList();
        foreach (var commonElement in commonElements)
        {
            nextPositionCandidates.Remove(commonElement);
        }
        
        if (nextPositionCandidates.Contains(starterPoint) && nextPositionCandidates.Count == 1)
        {
            totalSteps = totalSteps > steps ? totalSteps : steps + 1;
            return;
        }

        if (nextPositionCandidates.Count <= 0)
        {
            return;
        }
        
        foreach (var newPosition in nextPositionCandidates)
        {
            DoOneStep(newPosition, steps + 1, currentPathVisitedCells);
        }
            
        //Console.WriteLine($"{currentPosition.Item1},{currentPosition.Item2},{currentPosition.Item3}");

        totalSteps = totalSteps > steps ? totalSteps : steps + 1;
        longerVisitedCell = currentPathVisitedCells;
    }

    private List<(char, int, int)> GetNextPositionCandidates((char, int, int) currentPosition)
    {
        var candidates = new List<(char, int, int)>();
        
        var xMazeSize = mazeMatrix.GetLength(1) - 1; 
        var yMazeSize = mazeMatrix.GetLength(0) - 1;
        
        // Upper
        if (currentPosition.Item3 + 1 <= xMazeSize)
        {
            var upperValue = mazeMatrix[currentPosition.Item2, currentPosition.Item3 + 1];
            if (!upperValue.Equals(InvalidValue) && allowedPipesGoUpper.Contains(upperValue))
            {
                candidates.Add((upperValue, currentPosition.Item2, currentPosition.Item3 + 1));
            }
        }
        
        // Lower
        if (currentPosition.Item3 - 1 >= 0)
        {
            var lowerValue = mazeMatrix[currentPosition.Item2, currentPosition.Item3 - 1];
            if (!lowerValue.Equals(InvalidValue) && allowedPipesGoDown.Contains(lowerValue))
            {
                candidates.Add((lowerValue, currentPosition.Item2, currentPosition.Item3 - 1));
            }
        }
        
        // Left
        if (currentPosition.Item2 - 1 >= 0)
        {
            var leftValue = mazeMatrix[currentPosition.Item2 - 1, currentPosition.Item3];
            if (!leftValue.Equals(InvalidValue) && allowedPipesLeft.Contains(leftValue))
            {
                candidates.Add((leftValue, currentPosition.Item2 - 1, currentPosition.Item3));
            }
        }
        
        // Right
        if (currentPosition.Item2 + 1 <= yMazeSize)
        {
            var rightValue = mazeMatrix[currentPosition.Item2 + 1, currentPosition.Item3];
            if (!rightValue.Equals(InvalidValue) && allowedPipesRight.Contains(rightValue))
            {
                candidates.Add((rightValue, currentPosition.Item2 + 1, currentPosition.Item3));
            }
        }

        return candidates;
    }
}