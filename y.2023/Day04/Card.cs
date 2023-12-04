namespace Day04;

public class Card
{
    public int CardId { get; init; }
    
    public List<int>? WinningNumbers { get; init; }
    
    public List<int>? Numbers { get; init; }

    public int Amount { get; set; }
}