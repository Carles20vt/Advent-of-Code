namespace Day03;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        try
        {
            /*
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
            */

            var filePath = "./InputExample_Part01.txt";
            filePath = "./Input.txt";
            //filePath = "./Input_Carles.txt";

            var partNumberLines = File.ReadAllLines(filePath);

            var validPartNumbersSumValue = PartNumberChecker.GetValidPartNumbers(partNumberLines).Sum();
            
            Console.WriteLine($"The valid part numbers sum value is: {validPartNumbersSumValue}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}