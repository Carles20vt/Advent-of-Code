namespace Day03;

public static class GearChecker
{
    private const char GearSymbol = '*';

    private static Tuple<int, int, int> _lastAdjacentSymbolPosition = new (-1, -1, -1);
    
    public static IEnumerable<int> GetValidGearRatios(IEnumerable<string> partNumberLines)
    {
        var validGearRatios = new List<int>();
        
        var partNumberMatrix = ListToMatrix(partNumberLines.ToList());

        var files = partNumberMatrix.GetLength(0);
        var columns = partNumberMatrix.GetLength(1);

        for (var file = 0; file < files; file++)
        {
            _lastAdjacentSymbolPosition = new Tuple<int, int, int>(-1, -1, -1);
            
            for (var column = 0; column < columns; column++)
            {
                var currentChar = partNumberMatrix[file, column];

                var isGear = currentChar == GearSymbol;
                if (!isGear)
                {
                    continue;
                }
                
                _lastAdjacentSymbolPosition = new Tuple<int, int, int>(-1, -1, -1);
                
                var adjacentNumbers = GetAdjacentNumbers(file, column, partNumberMatrix);
                if (adjacentNumbers.Count != 2)
                {
                    continue;
                }

                Console.WriteLine($"Numbers to multiplied: {adjacentNumbers[0]} - {adjacentNumbers[1]} --> {adjacentNumbers[0].Item1 * adjacentNumbers[1].Item1}");

                var gearRatio = adjacentNumbers.Aggregate(1, (current, number) => current * number.Item1);

                validGearRatios.Add(gearRatio);
            }
        }
        
        return validGearRatios;
    }
    
    private static List<(int, int, int)> GetAdjacentNumbers(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var adjacentNumbers = new List<(int, int, int)>
        {
            GetAdjacentSymbolAtLeft(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentSymbolAtLeftUpside(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentNumberAtUpside(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentSymbolAtRightUpside(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentSymbolAtRight(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentSymbolAtRightDownside(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentNumberAtDownside(fileIndex, columnIndex, partNumberMatrix),
            GetAdjacentSymbolAtLeftDownside(fileIndex, columnIndex, partNumberMatrix)
        };

        var validAdjacentNumbers = adjacentNumbers.Where(x => x.Item1 != -1).ToList();
        return validAdjacentNumbers;
    }

    private static (int, int, int) GetAdjacentSymbolAtLeft(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        if (columnIndex > 0)
        {
            return GetValidNumberAtPosition(partNumberMatrix, fileIndex, columnIndex - 1);
        }
        
        return (-1, -1, -1);
    }
    
    private static (int, int, int) GetAdjacentSymbolAtLeftUpside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        if (columnIndex <= 0 || fileIndex <= 0)
        {
            return (-1, -1, -1);
        }

        return GetValidNumberAtPosition(partNumberMatrix, fileIndex - 1, columnIndex - 1);
    }
    
    private static (int, int, int) GetAdjacentSymbolAtLeftDownside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;
        if (columnIndex <= 0 || fileIndex >= maxFileIndex)
        {
            return (-1, -1, -1);
        }

        return GetValidNumberAtPosition(partNumberMatrix, fileIndex + 1, columnIndex - 1);
    }
    
    private static (int, int, int) GetAdjacentSymbolAtRight(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxColumnIndex = partNumberMatrix.GetLength(1) - 1;

        if (columnIndex < maxColumnIndex)
        {
            return GetValidNumberAtPosition(partNumberMatrix, fileIndex, columnIndex + 1);
        }

        return (-1, -1, -1);
    }
    
    private static (int, int, int) GetAdjacentSymbolAtRightUpside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxColumnIndex = partNumberMatrix.GetLength(1) - 1;

        if (columnIndex >= maxColumnIndex || fileIndex <= 0)
        {
            return (-1, -1, -1);
        }

        return GetValidNumberAtPosition(partNumberMatrix, fileIndex -1, columnIndex + 1);
    }
    
    private static (int, int, int) GetAdjacentSymbolAtRightDownside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;
        var maxColumnIndex = partNumberMatrix.GetLength(1) - 1;

        if (columnIndex >= maxColumnIndex || fileIndex >= maxFileIndex)
        {
            return (-1, -1, -1);
        }

        return GetValidNumberAtPosition(partNumberMatrix, fileIndex + 1, columnIndex + 1);
    }
    
    private static (int, int, int) GetAdjacentNumberAtDownside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;
        if (fileIndex < maxFileIndex)
        {
            return GetValidNumberAtPosition(partNumberMatrix, fileIndex + 1, columnIndex);
        }
        
        return (-1, -1, -1);
    }

    private static (int, int, int) GetAdjacentNumberAtUpside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;

        if (fileIndex > 0)
        {
            return GetValidNumberAtPosition(partNumberMatrix, fileIndex - 1, columnIndex);
        }
        
        return (-1, -1, -1);
    }

    private static (int, int, int) GetValidNumberAtPosition(char[,] partNumberMatrix, int file, int column)
    {
        var charToAnalyze = partNumberMatrix[file, column];
        var isDigit = char.IsDigit(charToAnalyze);
        var number = (-1, -1, -1);

        if (!isDigit)
        {
            return number;
        }
        
        number = ExtractValidNumber(file, column, partNumberMatrix);
        
        var isAlreadyExtracted = IsNumberWithValidSymbolAlreadyEvaluated(number.Item1, number.Item2, number.Item3);
        
        return isAlreadyExtracted ? (-1, -1, -1) : number;
    }

    private static bool IsNumberWithValidSymbolAlreadyEvaluated(int evaluatedNumber, int startIndex, int endIndex)
    {
        if (_lastAdjacentSymbolPosition.Item1 == evaluatedNumber &&
            _lastAdjacentSymbolPosition.Item2 == startIndex &&
            _lastAdjacentSymbolPosition.Item3 == endIndex)
        {
            return true;
        }
        
        _lastAdjacentSymbolPosition = new Tuple<int, int, int>(evaluatedNumber, startIndex, endIndex);
        
        return false;
    }

    private static (int, int, int) ExtractValidNumber(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var columns = partNumberMatrix.GetLength(1);
        var file = new char[columns];
        var startIndex = -1;
        var endIndex = -1;

        for (var column = 0; column < columns; column++)
        {
            file[column] = partNumberMatrix[fileIndex, column];
        }
        
        var validNumber = file[columnIndex].ToString();

        for (var c = columnIndex; c > 0; c--)
        {
            var indexToCheck = c - 1;
            var charToCheck = file[indexToCheck];
            
            if (char.IsDigit(charToCheck))
            {
                validNumber = validNumber.Insert(0, charToCheck.ToString());
                continue;
            }

            startIndex = indexToCheck;
            break;
        }
        
        for (var c = columnIndex; c < columns - 1 ; c++)
        {
            var indexToCheck = c + 1;
            var charToCheck = file[indexToCheck];
            
            if (char.IsDigit(charToCheck))
            {
                validNumber = validNumber.Insert(validNumber.Length, charToCheck.ToString());
                continue;
            }

            endIndex = indexToCheck;
            break;
        }

        var isParsed = int.TryParse(validNumber, out var parsedValidNumber);

        var parsedNumber= isParsed ? parsedValidNumber : 0;

        return (parsedNumber, startIndex, endIndex);
    }
    
    private static char[,] ListToMatrix(IReadOnlyList<string> list)
    {
        var files = list.Count;
        var columns = list[0].Length;

        var matrix = new char[files, columns];

        for (var i = 0; i < files; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                matrix[i, j] = list[i][j];
            }
        }

        return matrix;
    }
}