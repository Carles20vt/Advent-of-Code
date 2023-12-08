namespace Day07;

public class Card
{
    private readonly List<char> _labelList = new List<char>()
    {
        '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
    };

    public Card(char label)
    {
        Label = label;
    }
    
    public char Label { get; set; }

    public int Strength => _labelList.IndexOf(Label);

    private int GetStrength(char labelToCheck)
    {
        return _labelList.IndexOf(labelToCheck);
    }
}