﻿using System.Diagnostics;

namespace Day15;

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
        //var lensLibrary = new LensLibrary(allLines.ToList());
        //var totalSumOfHash = lensLibrary.CalculateTotalSumOfHash();
        stopwatch.Stop();

        //Console.WriteLine($"\n[Part 1] - The sum oh all HASH algorithm results is: {totalSumOfHash} - {stopwatch.ElapsedMilliseconds}ms.\n");
    }
}