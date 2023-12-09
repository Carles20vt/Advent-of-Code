using System.Diagnostics;

namespace Day08;

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

            var mapLines = File.ReadAllLines(filePath);

            stopwatch.Start();
            var hauntedWasteland = new HauntedWasteland(mapLines);
            var totalSteps = hauntedWasteland.GetStepsToEscape();
            stopwatch.Stop();
            
            Console.WriteLine($"\n[Part 1] - Required {totalSteps} Steps to reach ZZZ - {stopwatch.ElapsedMilliseconds}ms.\n");

            stopwatch.Start();
            hauntedWasteland = new HauntedWasteland(mapLines);
            var totalStepsPart2 = hauntedWasteland.GetStepsToEscapePart02Lcm();
            stopwatch.Stop();
            
            Console.WriteLine($"[Part 2] - Required {totalStepsPart2} Steps before you're only on nodes that end with Z - {stopwatch.ElapsedMilliseconds}ms.\n"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}