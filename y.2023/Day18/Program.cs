using System.Diagnostics;

namespace Day18;

internal abstract class Program
{
    private static void Main(string[] args)
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

        var allLines = File.ReadAllLines(filePath);
        
        stopwatch.Start();
        var lavaductLagoon = new LavaductLagoon(allLines);
        var cubicMeters = lavaductLagoon.CalculateCubicMeters();
        stopwatch.Stop();
        Console.WriteLine($"\n[Part 1] - The total cubic meters of lava it could hold are: {cubicMeters} - {stopwatch.ElapsedMilliseconds}ms.\n");
        
        stopwatch.Restart();
        lavaductLagoon = new LavaductLagoon(allLines, true);
        cubicMeters = lavaductLagoon.CalculateCubicMeters();
        stopwatch.Stop();
        Console.WriteLine($"\n[Part 2] - The total cubic meters of lava it could hold are: {cubicMeters} - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}