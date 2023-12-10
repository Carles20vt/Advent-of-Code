using System.Diagnostics;

namespace Day10;

internal abstract class Program
{
    private static void Main(string[] args)
    { /*
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

        var lectureLines = File.ReadAllLines(filePath);
/*
        stopwatch.Start();
        var pipeMaze = new PipeMaze(lectureLines.ToList());
        var farthestPoint = pipeMaze.GetFarthestPoint();
        stopwatch.Stop();
        
        Console.WriteLine($"\n[Part 1] - The required steps to reach the farthest point is: {farthestPoint} - {stopwatch.ElapsedMilliseconds}ms.\n");
*/     
        stopwatch.Start();
        var pipeMaze = new PipeMaze(lectureLines.ToList());
        var enclosedTiles = pipeMaze.GetEnclosedTiles();
        stopwatch.Stop();

        Console.WriteLine($"\n[Part 2] - The are {enclosedTiles} tiles enclosed by the loop - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}