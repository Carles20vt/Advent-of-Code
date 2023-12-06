namespace Day03;

public static class PartNumberChecker
{
    private const char Separator = '.';

    private static Tuple<int, int, int> _lastAdjacentSymbolPosition = new (-1, -1, -1);
    
    public static List<int> GetValidPartNumbers(IList<string> partNumberLines)
    {
        var validPartNumbers = new List<int>();
        
        var partNumberMatrix = ListToMatrix(partNumberLines.ToList());

        var files = partNumberMatrix.GetLength(0);
        var columns = partNumberMatrix.GetLength(1);

        for (var file = 0; file < files; file++)
        {
            var currentValidNumbersByFile = new List<int>();
            _lastAdjacentSymbolPosition = new Tuple<int, int, int>(-1, -1, -1);
            
            for (var column = 0; column < columns; column++)
            {
                var currentChar = partNumberMatrix[file, column];
                
                var isDigit = char.IsDigit(currentChar);
                if (!isDigit)
                {
                    continue;
                }
                
                if (currentChar == Separator)
                {
                    _lastAdjacentSymbolPosition = new Tuple<int, int, int>(-1, -1, -1);
                }
                
                var evaluatedNumber = ExtractValidNumber(file, column, columns, partNumberMatrix);
                
                var hasAdjacentSymbol = HasAdjacentSymbol(file, column, partNumberMatrix);
                if (!hasAdjacentSymbol)
                {
                    continue;
                }
                
                var isNumberWithValidSymbolAlreadyEvaluated = IsNumberWithValidSymbolAlreadyEvaluated(evaluatedNumber.Item1, evaluatedNumber.Item2, evaluatedNumber.Item3);
                if (isNumberWithValidSymbolAlreadyEvaluated)
                {
                    continue;
                }
                    
                currentValidNumbersByFile.Add(evaluatedNumber.Item1);
            }

            if (currentValidNumbersByFile.Count > 0)
            {
                validPartNumbers.AddRange(currentValidNumbersByFile);
            }
        }
        
        foreach (var validNum in validPartNumbers)
        {
            Console.WriteLine($"Added valid number: {validNum}");
        }

        return validPartNumbers;
    }
    
    private static bool HasAdjacentSymbol(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        return HasAdjacentSymbolAtLeft(fileIndex, columnIndex, partNumberMatrix) ||
               HasAdjacentSymbolAtLeftUpside(fileIndex, columnIndex, partNumberMatrix) ||
               HasAdjacentSymbolAtLeftDownside(fileIndex, columnIndex, partNumberMatrix) || 
               HasAdjacentSymbolAtRight(fileIndex, columnIndex, partNumberMatrix) || 
               HasAdjacentSymbolAtRightUpside(fileIndex, columnIndex, partNumberMatrix) || 
               HasAdjacentSymbolAtRightDownside(fileIndex, columnIndex, partNumberMatrix) || 
               HasAdjacentSymbolAtDownside(fileIndex, columnIndex, partNumberMatrix) || 
               HasAdjacentSymbolAtUpside(fileIndex, columnIndex, partNumberMatrix);
    }

    private static bool HasAdjacentSymbolAtLeft(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        return columnIndex > 0 && 
               CheckValidSymbolAtPosition(partNumberMatrix, fileIndex, columnIndex - 1);
    }
    
    private static bool HasAdjacentSymbolAtLeftUpside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        if (columnIndex <= 0 || fileIndex <= 0)
        {
            return false;
        }

        return CheckValidSymbolAtPosition(partNumberMatrix, fileIndex - 1, columnIndex - 1);
    }
    
    private static bool HasAdjacentSymbolAtLeftDownside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;
        if (columnIndex <= 0 || fileIndex >= maxFileIndex)
        {
            return false;
        }

        return CheckValidSymbolAtPosition(partNumberMatrix, fileIndex + 1, columnIndex - 1);
    }
    
    private static bool HasAdjacentSymbolAtRight(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxColumnIndex = partNumberMatrix.GetLength(1) - 1;

        return columnIndex < maxColumnIndex && 
               CheckValidSymbolAtPosition(partNumberMatrix, fileIndex, columnIndex + 1);
    }
    
    private static bool HasAdjacentSymbolAtRightUpside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxColumnIndex = partNumberMatrix.GetLength(1) - 1;

        if (columnIndex >= maxColumnIndex || fileIndex <= 0)
        {
            return false;
        }

        return CheckValidSymbolAtPosition(partNumberMatrix, fileIndex -1, columnIndex + 1);
    }
    
    private static bool HasAdjacentSymbolAtRightDownside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;
        var maxColumnIndex = partNumberMatrix.GetLength(1) - 1;

        if (columnIndex >= maxColumnIndex || fileIndex >= maxFileIndex)
        {
            return false;
        }

        return CheckValidSymbolAtPosition(partNumberMatrix, fileIndex + 1, columnIndex + 1);
    }
    
    private static bool HasAdjacentSymbolAtDownside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        return fileIndex > 0 && 
               CheckValidSymbolAtPosition(partNumberMatrix, fileIndex - 1, columnIndex);
    }

    private static bool HasAdjacentSymbolAtUpside(int fileIndex, int columnIndex, char[,] partNumberMatrix)
    {
        var maxFileIndex = partNumberMatrix.GetLength(0) - 1;

        return fileIndex < maxFileIndex && 
               CheckValidSymbolAtPosition(partNumberMatrix, fileIndex + 1, columnIndex);
    }

    private static bool CheckValidSymbolAtPosition(char[,] partNumberMatrix, int file, int column)
    {
       return IsValidSymbol(partNumberMatrix[file, column]);
    }
    
    private static bool IsValidSymbol(char charToAnalyze)
    {
        var isDigit = char.IsDigit(charToAnalyze);
        var isSeparator = charToAnalyze.Equals(Separator);

        return !isDigit && !isSeparator;
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

    private static (int, int, int) ExtractValidNumber(int fileIndex,int columnIndex, int columns, char[,] partNumberMatrix)
    {
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

            startIndex = c;
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

            endIndex = c;
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