using System.Diagnostics;

namespace Day16;

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
        var lensLibrary = new TheFloorWillBeLava(allLines.ToList());
        var energizedTiles = lensLibrary.CountEnergizedTiles();
        stopwatch.Stop();
        Console.WriteLine($"\n[Part 1] - The tiles amount being energized are: {energizedTiles} - {stopwatch.ElapsedMilliseconds}ms.\n");
        
        stopwatch.Restart();
        var energizedTilesAmount = lensLibrary.GetBestEnergizedTiles();
        stopwatch.Stop();
        Console.WriteLine($"\n[Part 2] - The tiles best amount being energized are: {energizedTilesAmount} - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}