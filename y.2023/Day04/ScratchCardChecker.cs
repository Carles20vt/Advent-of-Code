namespace Day04;

public static class ScratchCardChecker
{
    public static int GetPoints(string cardLine)
    {
        var scratchCard = BuildScratchCard(cardLine);

        var points = 0;
        var isFirstMatch = true;

        if (scratchCard.Numbers != null)
        {
            foreach (var number in scratchCard.Numbers)
            {
                var isWinningNumber = scratchCard.WinningNumbers != null &&
                                      scratchCard.WinningNumbers.Contains(number);
                if (!isWinningNumber)
                {
                    continue;
                }

                if (isFirstMatch)
                {
                    isFirstMatch = false;
                    points = 1;

                    continue;
                }

                points *= 2;
            }
        }

        Console.WriteLine($"The card {scratchCard.CardId} has {points} points.");

        return points;
    }
    
    public static int GetTotalScratchCardsWonRefactored(string [] scratchCardsLines)
    {
        var scratchCards = scratchCardsLines.Select(BuildScratchCard).ToList();
        
        foreach (var scratchCard in scratchCards)
        {
            UpdateCardsAmounts(scratchCard, scratchCards);
        }

        return scratchCards.Sum(x => x.Amount) + scratchCards.Count;
    }

    private static void UpdateCardsAmounts(Card scratchCard, List<Card> scratchCards)
    {
        if (scratchCard.Numbers == null ||
            scratchCard.WinningNumbers == null)
        {
            return;
        }
        
        var wonCardsCounter = 1;
        foreach (var number in scratchCard.Numbers)
        {
            if (!scratchCard.WinningNumbers.Contains(number))
            {
                continue;
            }
            
            var wonCard = scratchCards.FirstOrDefault(x => x.CardId == scratchCard.CardId + wonCardsCounter);
            if (wonCard == null)
            {
                continue;
            }
                
            wonCard.Amount++;
            wonCardsCounter++;

            UpdateCardsAmounts(wonCard, scratchCards);
        }
    }

    public static int GetTotalScratchCardsWon(string [] scratchCardsLines)
    {
        var originalScratchCards = scratchCardsLines.Select(BuildScratchCard).ToList();
        var copiesScratchCards = new List<Card>();

        foreach (var originalScratchCard in originalScratchCards)
        {
            GetWonCopies(originalScratchCard, originalScratchCards, copiesScratchCards);
        }

        return originalScratchCards.Count + copiesScratchCards.Count;
    }

    private static void GetWonCopies(Card originalScratchCard, List<Card> originalScratchCards, List<Card> copiesScratchCards)
    {
        if (originalScratchCard.Numbers == null)
        {
            return;
        }
        
        var matchingNumbersCount = originalScratchCard.Numbers
            .Count(number => originalScratchCard.WinningNumbers != null && 
                             originalScratchCard.WinningNumbers.Contains(number));

        if (matchingNumbersCount <= 0)
        {
            return;
        }

        for (var i = 1; i <= matchingNumbersCount; i++)
        {
            var newCopyCard = originalScratchCards.FirstOrDefault(x => x.CardId == originalScratchCard.CardId + i);
            
            if (newCopyCard == null)
            {
                continue;
            }
            
            copiesScratchCards.Add(newCopyCard);

            GetWonCopies(newCopyCard, originalScratchCards, copiesScratchCards);
        }
    }

    private static Card BuildScratchCard(string cardLine)
    {
        var splitCardLine = cardLine.Split(':');
        var splitNumbers = splitCardLine[1].Split('|');

        var scratchCard = new Card
        {
            CardId = GetCardId(splitCardLine[0]),
            WinningNumbers = GetNumbers(splitNumbers[0]),
            Numbers = GetNumbers(splitNumbers[1])
        };

        return scratchCard;
    }
    
    private static int GetCardId(string idToParse)
    {
        var cardId = idToParse.ToLower().Replace("card", string.Empty).Trim();
        var isParsed = int.TryParse(cardId, out var parsedCardId);

        return isParsed ? parsedCardId : -1;
    }
    
    private static List<int> GetNumbers(string numbersToParse)
    {
        var splitNumbers = numbersToParse.Split(' ');
        var numbers = new List<int>();

        foreach (var number in splitNumbers)
        {
            var isParsed = int.TryParse(number, out var parsedNumber);
            
            if (isParsed)
            {
                numbers.Add(parsedNumber);
            }
        }
        
        return numbers;
    }
}