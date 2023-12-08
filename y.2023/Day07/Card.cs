namespace Day07;

public class Card
{
    private readonly List<char> _labelList = new List<char>()
    {
        '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
    };
    
    private readonly List<char> _labelListWithJoker = new List<char>()
    {
        'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'
    };

    public Card(char label, bool jokerActive)
    {
        Label = label;
        IsJokerActive = jokerActive;
    }
    
    public char Label { get; set; }

    public int Strength => IsJokerActive ? _labelListWithJoker.IndexOf(Label) : _labelList.IndexOf(Label);

    private bool IsJokerActive { set; get; }

    private int GetStrength(char labelToCheck)
    {
        return _labelList.IndexOf(labelToCheck);
    }
}