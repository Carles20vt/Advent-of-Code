using System.Diagnostics;

namespace Day06;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        try
        {
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
            
            var stopwatch = new Stopwatch();

            var gameLines = File.ReadAllLines(filePath);

            stopwatch.Start();
            var numberOfWaysPower = BoatRace.GetNumberOfWaysPower(gameLines);
            stopwatch.Stop();
            Console.WriteLine($"\nThe number of ways multiplied are [Part 1]: {numberOfWaysPower} - {stopwatch.ElapsedMilliseconds}ms.");
            
            stopwatch.Start();
            numberOfWaysPower = BoatRace.GetNumberOfWaysPowerPart2(gameLines);
            stopwatch.Stop();
            Console.WriteLine($"\nThe number of ways multiplied are [Part 2]: {numberOfWaysPower} - {stopwatch.ElapsedMilliseconds}ms.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}