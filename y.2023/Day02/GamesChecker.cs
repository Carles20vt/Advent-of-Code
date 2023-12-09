namespace Day02;

public static class GamesChecker
{
    private const int MaxRedCubes = 12;
    private const int MaxGreenCubes = 13;
    private const int MaxBlueCubes = 14;
    
    public static int GetValidGames(string gameLine)
    {
        var game = BuildGame(gameLine);

        if (game.BagSets == null)
        {
            return 0;
        }

        var invalidBags = game.BagSets.Where(x =>
            x.RedCubes > MaxRedCubes || x.BlueCubes > MaxBlueCubes || x.GreenCubes > MaxGreenCubes);

        if (!invalidBags.Any())
        {
            return game.GameId;
        }
        
        foreach (var invalidBag in invalidBags)
        {
            Console.WriteLine($"Game {game.GameId} not complains with the rule " +
                              $"{invalidBag.RedCubes}/{MaxRedCubes} Red - " +
                              $"{invalidBag.GreenCubes}/{MaxGreenCubes} Green - " +
                              $"{invalidBag.BlueCubes}/{MaxBlueCubes} Blue.");
        }
            
        return 0;
    }

    public static int GetGamePower(string gameLine)
    {
        var game = BuildGame(gameLine);

        if (game.BagSets == null)
        {
            return 0;
        }

        var redRequired = game.BagSets.Max(x => x.RedCubes);
        var greenRequired = game.BagSets.Max(x => x.GreenCubes);
        var blueRequired = game.BagSets.Max(x => x.BlueCubes);
        var powerValue = redRequired * greenRequired * blueRequired;
        
        Console.WriteLine($"Game {game.GameId} required at least " +
                          $"{redRequired} red, " +
                          $"{greenRequired} green, and " +
                          $"{blueRequired} blue cubes. " +
                          $"Power value: {powerValue}");

        return powerValue;
    }

    private static Game BuildGame(string gameLine)
    {
        var gameId = GetGameId(gameLine);
        var bagSets = GetBagSets(gameLine);

        return new Game
        {
            GameId = gameId,
            BagSets = bagSets
        };
    }

    private static int GetGameId(string gameLine)
    {
        var gameId = gameLine.Split(':')[0].ToLower().Replace("game", string.Empty).Trim();
        var isParsed = int.TryParse(gameId, out var parsedId);

        return isParsed ? parsedId : 0;
    }

    private static List<BagSet> GetBagSets(string gameLine)
    {
        var gameBagSets = gameLine.Split(':')[1].ToLower();

        var bagSetsSplit = gameBagSets.Split(';');

        return bagSetsSplit.Select(BuildBagSet).ToList();
    }

    private static BagSet BuildBagSet(string bagSetText)
    {
        var bagSetSplit = bagSetText.Split(',');
        var redAmount = 0;
        var greenAmount = 0;
        var blueAmount = 0;
        
        foreach (var cubeInfo in bagSetSplit)
        {
            var redCalculated = GetCubeAmount(cubeInfo, "red");
            var greenCalculated = GetCubeAmount(cubeInfo, "green");
            var blueCalculated = GetCubeAmount(cubeInfo, "blue");

            redAmount += redCalculated > 0 ? redCalculated : 0;
            greenAmount += greenCalculated > 0 ? greenCalculated : 0;
            blueAmount += blueCalculated > 0 ? blueCalculated : 0;
        }

        var bagSet = new BagSet
        {
            RedCubes = redAmount,
            GreenCubes = greenAmount,
            BlueCubes = blueAmount
        };

        return bagSet;
    }

    private static int GetCubeAmount(string cubeInfo, string cubeColor)
    {
        if (!cubeInfo.Contains(cubeColor))
        {
            return 0;
        }

        var amountText = cubeInfo.Replace(cubeColor, string.Empty).Trim();

        var isAmountValueValid = int.TryParse(amountText, out var amount);

        return !isAmountValueValid ? 0 : amount;
    }
}