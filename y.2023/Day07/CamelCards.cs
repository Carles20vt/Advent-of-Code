using System.Text.RegularExpressions;

namespace Day07;

public class CamelCards
{
    private List<Hand> HandList;
    public CamelCards(string[] cardLines)
    {
        HandList = new List<Hand>();
        BuildHands(cardLines);
    }

    public long GetNumberOfTotalWinnings()
    {
        PrintResult();
            
        return HandList.Sum(hand => hand.Winning);
    }

    private void PrintResult()
    {
        foreach (var hand in HandList)
        {
            var cards = hand.Cards.Aggregate(string.Empty, (current, card) => current + card.Label);

            Console.WriteLine($"Cards: {cards} - Type: {hand.HandType} - Bid: {hand.Bid} - Rank: {hand.Rank} - Strength {hand.Strength}");
        }
    }

    private void BuildHands(string[] cardLines)
    {
        var fiveOfKindHands = new List<Hand>();
        var fourOfKindHands = new List<Hand>();
        var fullHouseHands = new List<Hand>();
        var threeOfKindHands = new List<Hand>();
        var twoPairHands = new List<Hand>();
        var onePairHands = new List<Hand>();
        var highCardHands = new List<Hand>();
        
        foreach (var line in cardLines)
        {
            var newHand = BuildHand(line);

            switch (newHand.HandType)
            {
                case HandType.HighCard:
                    highCardHands.Add(newHand);
                    highCardHands = OrderByStrongestCards(highCardHands);
                    continue;
                case HandType.OnePair:
                    onePairHands.Add(newHand);
                    onePairHands = OrderByStrongestCards(onePairHands);
                    continue;
                case HandType.TwoPair:
                    twoPairHands.Add(newHand);
                    twoPairHands = OrderByStrongestCards(twoPairHands);
                    continue;
                case HandType.ThreeOfKind:
                    threeOfKindHands.Add(newHand);
                    threeOfKindHands = OrderByStrongestCards(threeOfKindHands);
                    continue;
                case HandType.FullHouse:
                    fullHouseHands.Add(newHand);
                    fullHouseHands = OrderByStrongestCards(fullHouseHands);
                    continue;
                case HandType.FourOfKind:
                    fourOfKindHands.Add(newHand);
                    fourOfKindHands = OrderByStrongestCards(fourOfKindHands);
                    continue;
                case HandType.FiveOfAKind:
                    fiveOfKindHands.Add(newHand);
                    fiveOfKindHands = OrderByStrongestCards(fiveOfKindHands);
                    continue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        var orderedHands = new List<Hand>();
        
        orderedHands = orderedHands
            .Union(fiveOfKindHands)
            .Union(fourOfKindHands)
            .Union(fullHouseHands)
            .Union(threeOfKindHands)
            .Union(twoPairHands)
            .Union(onePairHands)
            .Union(highCardHands)
            .ToList();

        ApplyRankOrder(orderedHands);
        
        HandList.Clear();
        HandList = orderedHands;
    }

    private void ApplyRankOrder(List<Hand> hands)
    {
        var orderedHandsAmount = hands.Count;

        foreach (var hand in hands)
        {
            hand.Rank = orderedHandsAmount;
            orderedHandsAmount--;
        }
    }

    private List<Hand> OrderByStrongestCards(List<Hand> handCards)
    {
        var maxAmountOfCards = handCards.Count;
        
        if (maxAmountOfCards < 2)
        {
            return handCards;
        }

        var orderedHandCards = handCards
            .OrderByDescending(hand => hand.Cards?[0].Strength)
            .ThenByDescending(hand => hand.Cards?[1].Strength)
            .ThenByDescending(hand => hand.Cards?[2].Strength)
            .ThenByDescending(hand => hand.Cards?[3].Strength)
            .ThenByDescending(hand => hand.Cards?[4].Strength)
            .ToList();

        for (var i = 0; i < maxAmountOfCards; i++)
        {
            orderedHandCards[i].Strength = maxAmountOfCards - i;
        }

        return orderedHandCards;
    }
    
    private Hand BuildHand(string line)
    {
        var labelsString = line.Substring(0, 5).Trim().ToUpper();
        var amountString = line.Substring(5, line.Length - 5).Trim().ToUpper();
        
        var labels = labelsString.ToCharArray();
        var cards = labels.Select(label => new Card(label)).ToList();
        var amount = int.Parse(amountString);
        
        var hand = new Hand
        {
            Cards = cards,
            Bid = amount,
            HandType = DetermineHandType(labelsString),
            Rank = 1,
            Strength = 1
        };

        return hand;
    }

    private HandType DetermineHandType(string labels)
    {
        var isFiveOfAKind = Regex.IsMatch(labels, @"^(\w)\1{4}$");
        if (isFiveOfAKind)
        {
            return HandType.FiveOfAKind;
        }
        
        if (IsFourOfKind(labels))
        {
            return HandType.FourOfKind;
        }
        
        if (IsFullHouse(labels))
        {
            return HandType.FullHouse;
        }
        
        if (IsThreeOfKind(labels))
        {
            return HandType.ThreeOfKind;
        }
        
        if (IsTwoPair(labels))
        {
            return HandType.TwoPair;
        }
        
        if (IsOnePair(labels))
        {
            return HandType.OnePair;
        }
        
        if (IsHighCard(labels))
        {
            return HandType.HighCard;
        }
        
        return HandType.HighCard;
    }
    
    private bool IsFourOfKind(string labels)
    {
        var labelList = labels.ToCharArray().ToList();

        var repeatedLabel = FindRepeatedLabel(labelList, 4);
        if (repeatedLabel.Equals('*'))
        {
            return false;
        }

        return true;
    }

    private bool IsFullHouse(string labels)
    {
        var labelList = labels.ToCharArray().ToList();

        var labelOne = labelList[0];
        var labelTwo = labelList.FirstOrDefault(x => !x.Equals(labelOne));
        var labelOneAmount = labelList.Count(x => x.Equals(labelOne));
        var labelTwoAmount = labelList.Count(x => x.Equals(labelTwo));
        
        return (labelOneAmount == 3 && labelTwoAmount == 2) ||
               (labelOneAmount == 2 && labelTwoAmount == 3);
    }
    
    private bool IsThreeOfKind(string labels)
    {
        var labelList = labels.ToCharArray().ToList();
        
        var repeatedLabel = FindRepeatedLabel(labelList, 3);
        if (repeatedLabel.Equals('*'))
        {
            return false;
        }
        
        var uniqueLabel = FindUniqueLabel(labelList);
        if (uniqueLabel.Equals('*'))
        {
            return false;
        }

        return true;
    }
    
    private bool IsTwoPair(string labels)
    {
        var labelList = labels.ToCharArray().ToList();

        var uniqueLabel = FindUniqueLabel(labelList);
        if (uniqueLabel.Equals('*'))
        {
            return false;
        }

        labelList.Remove(uniqueLabel);
        
        var labelOne = labelList[0];
        var labelTwo = labelList.FirstOrDefault(x => !x.Equals(labelOne));
        
        var labelOneAmount = labelList.Count(x => x.Equals(labelOne));
        var labelTwoAmount = labelList.Count(x => x.Equals(labelTwo));
        
        return labelOneAmount == 2 && labelTwoAmount == 2;
    }
    
    private bool IsOnePair(string labels)
    {
        var labelList = labels.ToCharArray().ToList();
        
        var repeatedLabel = FindRepeatedLabel(labelList, 2);
        if (repeatedLabel.Equals('*'))
        {
            return false;
        }

        labelList.Remove(repeatedLabel);

        foreach (var label in labelList)
        {
            var isUnique = IsLabelUnique(labelList, label);
            if (!isUnique)
            {
                return false;
            }
        }

        return true;
    }
    
    private bool IsHighCard(string labels)
    {
        var labelList = labels.ToCharArray().ToList();

        foreach (var label in labelList)
        {
            var isUnique = IsLabelUnique(labelList, label);
            if (!isUnique)
            {
                return false;
            }
        }

        return true;
    }

    private static char FindUniqueLabel(List<char> labelList)
    {
        return FindRepeatedLabel(labelList, 1);
    }

    private static char FindRepeatedLabel(List<char> labelList, int repetitions)
    {
        foreach (var label in labelList)
        {
            var labelAmount = labelList.Count(x => x.Equals(label));
            if (labelAmount == repetitions)
            {
                return label;
            }
        }

        return '*';
    }

    private bool IsLabelUnique(List<char> labelList, char labelToCheck)
    {
        foreach (var label in labelList)
        {
            var labelAmount = labelList.Count(x => x.Equals(labelToCheck));
            if (labelAmount > 1)
            {
                return false;
            }
        }

        return true;
    }
}