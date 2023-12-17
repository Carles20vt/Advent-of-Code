using System.Drawing;

namespace Day17;

public class PointWithHeatLoss
{
    public Point Point { get; }
    public int HeatLoss { get; }

    public PointWithHeatLoss(Point point, int heatLoss)
    {
        Point = point;
        HeatLoss = heatLoss;
    }
}