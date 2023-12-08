using System.Diagnostics;

namespace Day08;

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
        
            var stopwatch = new Stopwatch();

            var mapLines = File.ReadAllLines(filePath);

            stopwatch.Start();
            var hauntedWasteland = new HauntedWasteland(mapLines);
            var totalSteps = hauntedWasteland.GetStepsToSoEscape();
            stopwatch.Stop();
            
            Console.WriteLine($"\n[Part 1] - Required {totalSteps} Steps to reach ZZZ - {stopwatch.ElapsedMilliseconds}ms.\n"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}