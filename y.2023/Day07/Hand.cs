namespace Day07;

public class Hand
{
    public int Strength { get; set; }
    
    public List<Card>? Cards { get; set; }
    
    public HandType HandType { get; set; }
    
    public int Bid { get; set; }
    
    public int Rank { get; set; }

    public int Winning => Bid * Rank;
}