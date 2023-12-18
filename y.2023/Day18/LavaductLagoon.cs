using System.Drawing;

namespace Day18;

public class LavaductLagoon
{
    private readonly Dictionary<Point, Color> levelMap;
    private readonly List<Point> polygonPoints;
    
    public LavaductLagoon(string[] allLines, bool decodeHex = false)
    {
        levelMap = new Dictionary<Point, Color>();
        polygonPoints = new List<Point>();

        if (decodeHex)
        {
            LoadLevelUsingHex(allLines);
        }
        else
        {
            LoadLevel(allLines);
        }
        
        //PrintLevel();
    }

    public long CalculateCubicMeters()
    {
        var cubicMeters = ShoelaceFormula();
        return cubicMeters;
    }

    /// <summary>
    /// https://www.themathdoctors.org/polygon-coordinates-and-areas/
    /// </summary>
    /// <returns>Area of irregular polygon</returns>
    private long ShoelaceFormula()
    {
        long s1 = 0;
        long s2 = 0;
        long pointsAmount = polygonPoints.Count - 1;

        for (var i = 0; i <= pointsAmount; i++)
        {
            long x1 = polygonPoints[i].X;

            var nextY = (i + 1);
            if (nextY > pointsAmount)
            {
                nextY = 0;
            }
                
            long y2 = polygonPoints[nextY].Y;

            s1 += x1 * y2;
        }
        
        for (var i = 0; i <= pointsAmount; i++)
        {
            var y1 = polygonPoints[i].Y;

            var nextX = (i + 1);
            if (nextX > pointsAmount)
            {
                nextX = 0;
            }
                
            long x2 = polygonPoints[nextX].X;

            s2 += x2 * y1;
        }

        var area = (Math.Abs(s1 - s2))/2;
        long trenchLen = levelMap.Count;
        
        return area + (trenchLen/2) + 1;
    }

    private void LoadLevel(string[] allLines)
    {
        var currentPoint = new Point();
        
        foreach (var line in allLines)
        {
            var splitLine = line.Split(' ');
            var directionChar = splitLine[0].Trim().ToUpper().ToCharArray()[0];
            var directionTimes = int.Parse(splitLine[1].Trim());
            var colorHex = splitLine[2].Replace("(", string.Empty).Replace(")", string.Empty).Trim().ToUpper();
            
            var color = GetColorFromHex(colorHex);
            var direction = GetDirectionFromCode(directionChar);
            
            for (var i = 0; i < directionTimes; i++)
            {
                currentPoint = new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y);
                
                levelMap.Add(currentPoint, color);
            }
            
            polygonPoints.Add(currentPoint);
        }
    }
    
    private void LoadLevelUsingHex(string[] allLines)
    {
        var currentPoint = new Point();
        
        foreach (var line in allLines)
        {
            var splitLine = line.Split(' ');
            var firstPartCode = splitLine[2].Trim().Substring(2, 5).ToUpper();
            var directionChar = splitLine[2].Substring(7, 1).ToCharArray()[0];
            var directionTimes = Convert.ToInt32(firstPartCode, 16);
            
            var colorHex = splitLine[2].Replace("(", string.Empty).Replace(")", string.Empty).Trim().ToUpper();
            
            var color = GetColorFromHex(colorHex);
            var direction = GetDirectionFromCode(directionChar);
            
            for (var i = 0; i < directionTimes; i++)
            {
                currentPoint = new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y);
                
                levelMap.Add(currentPoint, color);
            }
            
            polygonPoints.Add(currentPoint);
        }
    }

    private static Point GetDirectionFromCode(char directionChar)
    {
        return directionChar switch
        {
            'R' => new Point(1, 0),
            '0' => new Point(1, 0),
            'L' => new Point(-1, 0),
            '2' => new Point(-1, 0),
            'U' => new Point(0, 1),
            '3' => new Point(0, 1),
            'D' => new Point(0, -1),
            '1' => new Point(0, -1),
            _ => new Point(0, 0)
        };
    }

    private static Color GetColorFromHex(string colorHex)
    {
        var color = Color.FromArgb(
            int.Parse(colorHex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
            int.Parse(colorHex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
            int.Parse(colorHex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber)
        );

        return color;
    }

    private void PrintLevel()
    {
        var minRowIndex = levelMap.Min(point => point.Key.X);
        var maxRowIndex = levelMap.Max(point => point.Key.X);
        
        var minColumnIndex = levelMap.Min(point => point.Key.Y);
        var maxColumnIndex = levelMap.Max(point => point.Key.Y);
        
        for (var column = maxColumnIndex; column >= minColumnIndex; column--)
        {
            for (var file = minRowIndex; file <= maxRowIndex; file++)
            {
                var pointToShow = new Point(file, column);
                var isDug = levelMap.ContainsKey(pointToShow);
                var charToShow = isDug ? '#' : '.';
                
                Console.Write(charToShow);
            }
            
            Console.WriteLine(string.Empty);
        }
    }
}