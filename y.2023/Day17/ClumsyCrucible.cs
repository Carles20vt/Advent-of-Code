using System.Drawing;

namespace Day17;

public class ClumsyCrucible
{
    private readonly int totalRows;
    private readonly int totalColumns;
    
    private Dictionary<Point, int> heatMap;
    private Queue<Point> directionPointsTracking;
    
    public ClumsyCrucible(List<string> inputLines)
    {
        totalRows = inputLines[0].Length;
        totalColumns = inputLines.Count;
        heatMap = new Dictionary<Point, int>();
        directionPointsTracking = new Queue<Point>(3);
        
        LoadData(inputLines);
    }

    public int GetLastHeatLoss()
    {
        var startPosition = new Point(0, totalColumns - 1);
        var destinationPosition = new Point(totalRows - 1, 0);

        var heatLoss = MoveCrucible(startPosition, destinationPosition);
        
        return heatLoss;
    }

    private void LoadData(List<string> inputLines)
    {
        for (var file = totalColumns - 1; file >= 0; file--)
        {
            for (var column = 0; column < totalRows; column++)
            {
                var value = int.Parse(inputLines[column].ToCharArray()[file].ToString());
                var columnAtYValue = (totalRows - 1) - column;
                
                heatMap.Add( new Point(file, columnAtYValue), value);
            }
        }
    }

    private int MoveCrucible(Point startPosition, Point destinationPosition)
    {
        int[] directionX = { -1, 0, 1, 0 };
        int[] directionY = { 0, -1, 0, 1 };
        
        var heatLoss = new Dictionary<Point, int>();
        var previous = new Point[totalRows, totalColumns];
        
        // Initialize distances
        for (var x = 0; x < totalRows; x++)
        {
            for (var y = 0; y < totalColumns; y++)
            {
                heatLoss[new Point(x, y)] = int.MaxValue;
            }
        }

        heatLoss[startPosition] = heatMap[startPosition];
        directionPointsTracking.Enqueue(new Point(1, 0));

        var priorityQueue = new SortedSet<PointWithHeatLoss>(Comparer<PointWithHeatLoss>.Create((a, b) => a.HeatLoss.CompareTo(b.HeatLoss)))
        {
            new (startPosition, heatLoss[startPosition])
        };
        
        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Min;
            priorityQueue.Remove(current);

            for (var i = 0; i < 4; i++)
            {
                var directionPoint = new Point(directionX[i], directionY[i]);
                var destinationPoint = new Point(current.Point.X + directionPoint.X, current.Point.Y + directionPoint.Y);

                var isValidPoint = IsValidPoint(destinationPoint, directionPoint, current.Point);
                if (isValidPoint)
                {
                    int newHeatLoss = heatLoss[current.Point] + heatMap[destinationPoint];

                    if (newHeatLoss < heatLoss[destinationPoint])
                    {
                        heatLoss[destinationPoint] = newHeatLoss;
                        previous[destinationPoint.X, destinationPoint.Y] = current.Point;
                        
                        if (directionPointsTracking.Count >= 3)
                        {
                            directionPointsTracking.Dequeue();
                        }
        
                        directionPointsTracking.Enqueue(directionPoint);
                        
                        if (!priorityQueue.Add(new PointWithHeatLoss(destinationPoint, newHeatLoss)))
                        {
                            priorityQueue.Remove(new PointWithHeatLoss(destinationPoint, heatLoss[destinationPoint]));
                            priorityQueue.Add(new PointWithHeatLoss(destinationPoint, newHeatLoss));
                        }
                    }
                }
            }
        }
        
        var currentPoint = destinationPosition;
        var total = 0;


        while (currentPoint != startPosition)
        {
            var oldCurrent = currentPoint;
            currentPoint = previous[currentPoint.X, currentPoint.Y];
            
            Console.WriteLine($"({oldCurrent}) --> ({currentPoint})");
            
            total += heatMap[currentPoint];
        }

        //total -= heatMap[startPosition];
        //total -= heatMap[destinationPosition];

        return total;
    }
    
    private bool IsValidPoint(Point destinationPoint, Point directionPoint, Point currentPoint)
    {
        if (destinationPoint.Equals(currentPoint))
        {
            return false;
        }

        var repeatedDirectionsAmount = directionPointsTracking.Count(point => point.Equals(directionPoint));
        if (repeatedDirectionsAmount >= 3)
        {
            return false;
        }
        
        var isInBounds = destinationPoint.X >= 0 && destinationPoint.X < totalRows && destinationPoint.Y >= 0 && destinationPoint.Y < totalColumns;
        return isInBounds;
    }
}