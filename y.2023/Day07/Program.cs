using System.Diagnostics;

namespace Day07;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        try
        {
            /*
            if (args.Length < 1)
            {
                Console.WriteLine("No input file provided. Please provide one.");
                return;
            }

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }
            */
            var filePath = "./Input.txt";
            //filePath = "./InputExample.txt";
            //filePath = "./InputCarles.txt";
            
            
            var stopwatch = new Stopwatch();

            var cardLines = File.ReadAllLines(filePath);

            stopwatch.Start();
            var camelCards = new CamelCards(cardLines);
            var totalWinnings = camelCards.GetNumberOfTotalWinnings();
            stopwatch.Stop();
            
            Console.WriteLine($"\n[Part 1] - The total winnings are : {totalWinnings} - {stopwatch.ElapsedMilliseconds}ms.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}