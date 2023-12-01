namespace Day01;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        try
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No calibration input file provided. Please provide one.");
                return;
            }            

            var  filePath = args[0];
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }

            var calibratorLine = File.ReadAllLines(filePath);
            
            var calibrationValue = calibratorLine.Sum(GetCalibration);
            
            Console.WriteLine($"The Calibration value is: {calibrationValue}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static int GetCalibration(string lineCalibration)
    {
        var numbersFormattedText = GetAllNumbers(lineCalibration);
        
        var firstDigit = numbersFormattedText.FirstOrDefault();
        var lastDigit = numbersFormattedText.LastOrDefault();

        var calibrationValue = firstDigit ?? firstDigit;
        
        if (numbersFormattedText.Count > 1)
        {
            calibrationValue += lastDigit ?? lastDigit;
        }

        var isResultParsedCorrectly = int.TryParse(calibrationValue, out var result);

        Console.WriteLine($"The encrypted calibration line {lineCalibration} has a calibration value of: {result}");
        
        return isResultParsedCorrectly ? result : 0;
    }

    private static List<string> GetAllNumbers(string lineCalibration)
    {
        var spelledNumbers = new List<string>
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
        };
        
        var allowedDigits = new List<string>
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        var numbersFound = spelledNumbers
            .SelectMany(number =>
                lineCalibration
                    .Select((c, index) => new { Char = c, Index = index })
                    .Where(item => lineCalibration.IndexOf(number, item.Index, StringComparison.OrdinalIgnoreCase) == item.Index)
                    .Select(item => new { Number = number, PositionOnText = item.Index })
            )
            .Where(x => x.PositionOnText != -1)
            .ToList();
        
        var digitsFound = allowedDigits
            .SelectMany(number =>
                lineCalibration
                    .Select((c, index) => new { Char = c, Index = index })
                    .Where(item => lineCalibration.IndexOf(number, item.Index, StringComparison.OrdinalIgnoreCase) == item.Index)
                    .Select(item => new { Number = number, PositionOnText = item.Index })
            )
            .Where(x => x.PositionOnText != -1)
            .ToList();

        var allNumbers = numbersFound.Concat(digitsFound)
            .OrderBy(x => x.PositionOnText)
            .Select(x => x.Number)
            .ToList();

        // Repeat Digit When Only One Exists
        if (allNumbers.Count == 1 && digitsFound.Count == 1)
        {
            allNumbers.Add(digitsFound.FirstOrDefault().Number);
        }

        ReplaceNumbersToDigits(allNumbers, spelledNumbers);

        return allNumbers;
    }

    private static void ReplaceNumbersToDigits(List<string> allNumbers, List<string> spelledNumbers)
    {
        for (var i = 0; i < allNumbers.Count; i++)
        {
            var number = spelledNumbers.IndexOf(allNumbers[i]);
            if (number > 0)
            {
                allNumbers[i] = number.ToString();
            }
        }
    }
}