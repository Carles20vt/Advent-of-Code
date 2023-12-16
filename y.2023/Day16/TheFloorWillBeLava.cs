namespace Day16;

public class TheFloorWillBeLava
{
    private const char EmptySpace = '.';
    private const char MirrorUpward = '/';
    private const char MirrorDownward = '\\';
    private const char SplitterVertical = '|';
    private const char SplitterHorizontal = '-';
    
    private readonly int totalRows;
    private readonly int totalColumns;
    private readonly List<string> inputLines;
    
    private List<(int, int, int, int)> energizedTiles;
    private Dictionary<(int, int), (char, bool)> map;
    
    public TheFloorWillBeLava(List<string> inputLines)
    {
        this.inputLines = inputLines;
        totalRows = inputLines[0].Length;
        totalColumns = inputLines.Count;
        energizedTiles = new List<(int, int, int, int)>();
        map = new Dictionary<(int, int), (char, bool)>();
        LoadData();
    }

    public int CountEnergizedTiles()
    {
        var startPosition = (0, totalColumns - 1);
        var startDirection = (1, 0);

        MoveLight(startPosition, startDirection);

        var totalEnergizedTiles = map.Count(tile => tile.Value.Item2);

        return totalEnergizedTiles;
    }

    public int GetBestEnergizedTiles()
    {
        var totalEnergizedTiles = 0;
        
        var startDirection = (0, 1);
        for (var row = 0; row < totalRows; row++)
        {
            ResetCounters();
            MoveLight((row, 0), startDirection);
            var energizedTilesAmount = map.Count(tile => tile.Value.Item2);
            if (energizedTilesAmount > totalEnergizedTiles)
            {
                totalEnergizedTiles = energizedTilesAmount;
            }
        }
        

        startDirection = (0, -1);
        for (var row = 0; row < totalRows; row++)
        {
            ResetCounters();
            MoveLight((row, totalColumns - 1), startDirection);
            var energizedTilesAmount = map.Count(tile => tile.Value.Item2);
            if (energizedTilesAmount > totalEnergizedTiles)
            {
                totalEnergizedTiles = energizedTilesAmount;
            }
        }
        
        startDirection = (1, 0);
        for (var column = 0; column < totalColumns; column++)
        {
            ResetCounters();
            MoveLight((0, column), startDirection);
            var energizedTilesAmount = map.Count(tile => tile.Value.Item2);
            if (energizedTilesAmount > totalEnergizedTiles)
            {
                totalEnergizedTiles = energizedTilesAmount;
            }
        }
        
        startDirection = (-1, 0);
        for (var column = 0; column < totalColumns; column++)
        {
            ResetCounters();
            MoveLight((totalRows - 1, column), startDirection);
            var energizedTilesAmount = map.Count(tile => tile.Value.Item2);
            if (energizedTilesAmount > totalEnergizedTiles)
            {
                totalEnergizedTiles = energizedTilesAmount;
            }
        }

        return totalEnergizedTiles;
    }

    private void ResetCounters()
    {
        energizedTiles = new List<(int, int, int, int)>();
        LoadData();
    }

    private void LoadData()
    {
        map = new Dictionary<(int, int), (char, bool)>();
        
        for (var file = totalColumns - 1; file >= 0; file--)
        {
            for (var column = 0; column < totalRows; column++)
            {
                var value = inputLines[column].ToCharArray()[file];
                var columnAtYValue = (totalRows - 1) - column;
                
                map.Add((file, columnAtYValue), (value, false));
            }
        }
    }

    private void MoveLight((int, int) position, (int, int) direction)
    {
        while (true)
        {
            if (position.Item1 >= totalRows || position.Item1 < 0 || position.Item2 >= totalColumns || position.Item2 < 0)
            {
                return;
            }

            var isVisited = MarkTileAsVisited(position, direction);
            if (isVisited)
            {
                return;
            }

            var newDirection = direction;
            var currentTile = map[position].Item1;

            switch (currentTile)
            {
                case EmptySpace:
                    newDirection = direction;
                    break;
                case MirrorUpward:
                    newDirection = (direction.Item2, direction.Item1);
                    break;
                case MirrorDownward:
                    newDirection = (direction.Item2 * -1, direction.Item1 * -1);
                    break;
                case SplitterHorizontal when Math.Abs(direction.Item1) > 0:
                    newDirection = direction;
                    break;
                case SplitterHorizontal:
                {
                    newDirection = (1, 0);

                    var newDirectionB = (-1, 0);
                    var newPositionB = (position.Item1 + newDirectionB.Item1, position.Item2 + newDirectionB.Item2);

                    MoveLight(newPositionB, newDirectionB);
                    break;
                }
                case SplitterVertical when Math.Abs(direction.Item2) > 0:
                    newDirection = direction;
                    break;
                case SplitterVertical:
                {
                    newDirection = (0, 1);

                    var newDirectionB = (0, -1);
                    var newPositionB = (position.Item1 + newDirectionB.Item1, position.Item2 + newDirectionB.Item2);

                    MoveLight(newPositionB, newDirectionB);
                    break;
                }
            }

            var newPosition = (position.Item1 + newDirection.Item1, position.Item2 + newDirection.Item2);

            position = newPosition;
            direction = newDirection;
        }
    }

    private bool MarkTileAsVisited((int, int) position, (int, int) direction)
    {
        var currentTileData = (position.Item1, position.Item2, direction.Item1, direction.Item2);
        if (energizedTiles.Contains(currentTileData))
        {
            return true;
        }
        
        energizedTiles.Add(currentTileData);
        
        var currentTile = map[position].Item1;
        map.Remove(position);
        map.Add(position, (currentTile, true));

        return false;
    }
}