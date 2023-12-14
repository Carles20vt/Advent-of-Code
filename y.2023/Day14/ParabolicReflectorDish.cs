namespace Day14;

public class ParabolicReflectorDish
{
    private const char CubeShapedRock = '#';
    private const char RoundedRock = 'O';

    private HashSet<(int, int)> foundRoundedRocks;
    private List<(int, int)> foundCubeShapedRocks;
    private readonly int columnsAmount;
    private readonly int rowsAmount;
    
    public ParabolicReflectorDish(List<string> inputLines)
    {
        foundRoundedRocks = new HashSet<(int, int)>();
        foundCubeShapedRocks = new List<(int, int)>();
        columnsAmount = inputLines[0].Length;
        rowsAmount = inputLines.Count;
        
        LoadData(inputLines);
    }

    public long CalculateTotalLoadAtNorth()
    {
        TiltToNorth();

        var totalLoad = GetTotalLoadToNorth();

        return totalLoad;
    }

    public long CalculateTotalLoadAtNorthWithSpinCycle(int cycles)
    {
        var totalLoad = TiltToCycles(cycles);
        return totalLoad;
    }
    
    private long GetTotalLoadToNorth()
    {
        var load = 0;
        
        for (var column = columnsAmount; column >= 0; column--)
        {
            var amount = foundRoundedRocks.Count(x => x.Item2.Equals(column - 1)) * column;
            load += amount;
        }

        return load;
    }

    private long TiltToCycles(int cycles)
    {
        long totalLoad = 0;
        
        for (var cycle = 0; cycle <= cycles; cycle++)
        {
            TiltToNorth();
            TiltToWest();
            TiltToSouth();
            TiltToEast();
            
            totalLoad = GetTotalLoadToNorth();
            Console.WriteLine($"Cycle {cycle} finished --> {totalLoad}.");
        }

        return totalLoad;
    }

    private void LoadData(List<string> inputLines)
    {
        for (var file = columnsAmount - 1; file >= 0; file--)
        {
            for (var column = 0; column < rowsAmount; column++)
            {
                var value = inputLines[column].ToCharArray()[file];
                var columnAtYValue = (rowsAmount - 1) - column;
                
                if (value.Equals(RoundedRock))
                {
                    foundRoundedRocks.Add((file, columnAtYValue));
                }
                
                if (value.Equals(CubeShapedRock))
                {
                    foundCubeShapedRocks.Add((file, columnAtYValue));
                }
            }
        }
    }

    private void TiltToNorth()
    {
        for (var row = rowsAmount - 1; row >= 0; row--)
        {
            var lastEmptySpace = (-1, -1);
            
            for (var column = 0; column < columnsAmount; column++)
            {
                var columnAtYValue = (columnsAmount - 1) - column;

                var isRoundedRock = foundRoundedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(columnAtYValue));
                var isCubeShapedRock = foundCubeShapedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(columnAtYValue));
                var isEmptySpace = isRoundedRock == false && isCubeShapedRock == false;

                if (isEmptySpace && lastEmptySpace.Equals((-1, -1)))
                {
                    lastEmptySpace = (row, columnAtYValue);
                }

                if (isRoundedRock && !lastEmptySpace.Equals((-1, -1)))
                {
                    foundRoundedRocks.Remove((row, columnAtYValue));
                    foundRoundedRocks.Add(lastEmptySpace);

                    column += (columnAtYValue - lastEmptySpace.Item2);
                    
                    lastEmptySpace = (-1, -1);
                    continue;
                }

                if (isCubeShapedRock)
                {
                    lastEmptySpace = (-1, -1);
                }
            }
        }
    }

    private void TiltToSouth()
    {
        for (var row = rowsAmount - 1; row >= 0; row--)
        {
            var lastEmptySpace = (-1, -1);
            
            for (var column = 0; column < columnsAmount; column++)
            {
                var isRoundedRock = foundRoundedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(column));
                var isCubeShapedRock = foundCubeShapedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(column));
                var isEmptySpace = isRoundedRock == false && isCubeShapedRock == false;

                if (isEmptySpace && lastEmptySpace.Equals((-1, -1)))
                {
                    lastEmptySpace = (row, column);
                }

                if (isRoundedRock && !lastEmptySpace.Equals((-1, -1)))
                {
                    foundRoundedRocks.Remove((row, column));
                    foundRoundedRocks.Add(lastEmptySpace);

                    column -= (column - lastEmptySpace.Item2);
                    
                    lastEmptySpace = (-1, -1);
                    continue;
                }

                if (isCubeShapedRock)
                {
                    lastEmptySpace = (-1, -1);
                }
            }
        }
    }
    
    private void TiltToEast()
    {
        for (var column = 0; column < columnsAmount; column++)
        {
            var lastEmptySpace = (-1, -1);
            
            for (var row = rowsAmount - 1; row >= 0; row--)
            {
                var isRoundedRock = foundRoundedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(column));
                var isCubeShapedRock = foundCubeShapedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(column));
                var isEmptySpace = isRoundedRock == false && isCubeShapedRock == false;

                if (isEmptySpace && lastEmptySpace.Equals((-1, -1)))
                {
                    lastEmptySpace = (row, column);
                }

                if (isRoundedRock && !lastEmptySpace.Equals((-1, -1)))
                {
                    foundRoundedRocks.Remove((row, column));
                    foundRoundedRocks.Add(lastEmptySpace);

                    row += (lastEmptySpace.Item1 - row);
                    
                    lastEmptySpace = (-1, -1);
                    continue;
                }

                if (isCubeShapedRock)
                {
                    lastEmptySpace = (-1, -1);
                }
            }
        }
    }
    
    private void TiltToWest()
    {
        for (var column = 0; column < columnsAmount; column++)
        {
            var lastEmptySpace = (-1, -1);
            
            for (var row = 0; row < rowsAmount - 1; row++)
            {
                var isRoundedRock = foundRoundedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(column));
                var isCubeShapedRock = foundCubeShapedRocks.Any(x => x.Item1.Equals(row) && x.Item2.Equals(column));
                var isEmptySpace = isRoundedRock == false && isCubeShapedRock == false;

                if (isEmptySpace && lastEmptySpace.Equals((-1, -1)))
                {
                    lastEmptySpace = (row, column);
                }

                if (isRoundedRock && !lastEmptySpace.Equals((-1, -1)))
                {
                    foundRoundedRocks.Remove((row, column));
                    foundRoundedRocks.Add(lastEmptySpace);

                    row += (lastEmptySpace.Item1 - row);
                    
                    lastEmptySpace = (-1, -1);
                    continue;
                }

                if (isCubeShapedRock)
                {
                    lastEmptySpace = (-1, -1);
                }
            }
        }
    }
}