using System.Diagnostics;

namespace Day11;

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
        var cosmicExpansion = new CosmicExpansion(allLines.ToList());
        var farthestPoint = cosmicExpansion.GetShortestPathBetweenGalaxies();
        stopwatch.Stop();

        Console.WriteLine($"\n[Part 1] - The sum of the shortest path between all pair of galaxies is: {farthestPoint} - {stopwatch.ElapsedMilliseconds}ms.\n");

        stopwatch.Start();
        cosmicExpansion = new CosmicExpansion(allLines.ToList(), 999999);
        farthestPoint = cosmicExpansion.GetShortestPathBetweenGalaxies();
        stopwatch.Stop();

        Console.WriteLine($"\n[Part 2] - The sum of the shortest path between all pair of galaxies is: {farthestPoint} - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}