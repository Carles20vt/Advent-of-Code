using System.Diagnostics;

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
            
            var stopwatch = new Stopwatch();

            var scratchBoardLines = File.ReadAllLines(filePath);

            stopwatch.Start();
            var totalPoints = scratchBoardLines.Sum(ScratchCardChecker.GetPoints);
            stopwatch.Stop();
            Console.WriteLine($"The total points are: {totalPoints} - {stopwatch.ElapsedMilliseconds} ms");
            
            stopwatch.Restart();
            var totalScratchCardsWon = ScratchCardChecker.GetTotalScratchCardsWon(scratchBoardLines);
            stopwatch.Stop();
            Console.WriteLine($"\nThe total won scratchcards are: {totalScratchCardsWon} - {stopwatch.ElapsedMilliseconds} ms");
            
            stopwatch.Restart();
            totalScratchCardsWon = ScratchCardChecker.GetTotalScratchCardsWonRefactored(scratchBoardLines);
            stopwatch.Stop();
            Console.WriteLine($"\nThe total won scratchcards are: {totalScratchCardsWon} (Refactored) - {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}