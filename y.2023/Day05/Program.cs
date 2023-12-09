﻿using System.Diagnostics;

namespace Day05;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        try
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No games input file provided. Please provide one.");
                return;
            }

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }
            
            var stopwatch = new Stopwatch();

            var almanacLines = File.ReadAllLines(filePath);
            
            stopwatch.Start();
            var almanac = new Almanac(almanacLines);
            var lowestLocation = almanac.GetLowestLocation();
            stopwatch.Stop();
            Console.WriteLine($"\n[Part02] - The lowest location number is: {lowestLocation} - {stopwatch.ElapsedMilliseconds/1000}s.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}