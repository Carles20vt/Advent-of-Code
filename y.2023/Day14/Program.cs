using System.Diagnostics;

namespace Day14;

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
        //filePath = "./InputExample.txt";
        //filePath = "./InputExampleCarles.txt";
        
        var stopwatch = new Stopwatch();

        var allLines = File.ReadAllLines(filePath);
        
        stopwatch.Start();
        var parabolicReflectorDish = new ParabolicReflectorDish(allLines.ToList());
        var totalLoadAtNorth = parabolicReflectorDish.CalculateTotalLoadAtNorth();
        stopwatch.Stop();

        Console.WriteLine($"\n[Part 1] - The total load at north support beams is: {totalLoadAtNorth} - {stopwatch.ElapsedMilliseconds}ms.\n");
        
        stopwatch.Start();
        totalLoadAtNorth = parabolicReflectorDish.CalculateTotalLoadAtNorthWithSpinCycle(100);
        stopwatch.Stop();

        Console.WriteLine($"\n[Part 2] - The total load at north support beams after 1000M Cycles is: {totalLoadAtNorth} - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}