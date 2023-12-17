using System.Diagnostics;

namespace Day17;

internal abstract class Program
{
    private static void Main(string[] args)
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
        filePath = "./InputExample.txt";
        
        var stopwatch = new Stopwatch();

        var allLines = File.ReadAllLines(filePath);
        
        stopwatch.Start();
        var clumsyCrucible = new ClumsyCrucible(allLines.ToList());
        var lastHeatLoss = clumsyCrucible.GetLastHeatLoss();
        stopwatch.Stop();
        Console.WriteLine($"\n[Part 1] - The last heat loss it can incur is: {lastHeatLoss} - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}