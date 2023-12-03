namespace Day03;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        try
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No games input file provided. Please provide one.");
                return;
            }

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }            

            var partNumberLines = File.ReadAllLines(filePath);

            var validPartNumbersSumValue = PartNumberChecker.GetValidPartNumbers(partNumberLines).Sum();
            Console.WriteLine($"The valid part numbers sum value is: {validPartNumbersSumValue}");
            
            var validGearRatiosSumValue = GearChecker.GetValidGearRatios(partNumberLines).Sum();
            Console.WriteLine($"The valid gear ratios sum value is: {validGearRatiosSumValue}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}