using System.Diagnostics;

namespace Day09;

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

            var lectureLines = File.ReadAllLines(filePath);

            stopwatch.Start();
            var oasis = new Oasis(lectureLines);
            var sumFromLectures = oasis.GetSumFromLectures();
            stopwatch.Stop();
            
            Console.WriteLine($"\n[Part 1] - The sum of these extrapolated values are: {sumFromLectures} - {stopwatch.ElapsedMilliseconds}ms.\n");
            
            stopwatch.Restart();
            var sumFromLecturesBackwards = oasis.GetSumFromLecturesBackwards();
            stopwatch.Stop();
            
            Console.WriteLine($"[Part 2] - The sum of these backwards extrapolated values are: {sumFromLecturesBackwards} - {stopwatch.ElapsedMilliseconds}ms.\n");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}