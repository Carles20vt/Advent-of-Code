namespace Day02;

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

            var gameLines = File.ReadAllLines(filePath);

            var gamesSumValue = gameLines.Sum(GamesChecker.GetValidGames);
            Console.WriteLine($"The Valid Games IDs sum value is: {gamesSumValue}");

            var gamesSumPowerValue = gameLines.Sum(GamesChecker.GetGamePower);
            Console.WriteLine($"The Games Power sum value is: {gamesSumPowerValue}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}