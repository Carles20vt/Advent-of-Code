namespace Day04;

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

            var scratchBoardLines = File.ReadAllLines(filePath);

            var totalPoints = scratchBoardLines.Sum(ScratchCardChecker.GetPoints);
            Console.WriteLine($"The total points are: {totalPoints}");

            var totalScratchCardsWon = ScratchCardChecker.GetTotalScratchCardsWon(scratchBoardLines);
            Console.WriteLine($"\nThe total won scratchcards are: {totalScratchCardsWon}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}