namespace Day09;

public class Oasis
{
    private List<List<int>> lectureData;
    
    public Oasis(IEnumerable<string> inputReadLines)
    {
        lectureData = BuildLectureData(inputReadLines);
    }
    
    public int GetSumFromLectures()
    {
        return lectureData.Sum(GetNextValue);
    }
    
    public object GetSumFromLecturesBackwards()
    {
        return lectureData.Sum(GetBackwardsValue);
    }

    private static List<List<int>> BuildLectureData(IEnumerable<string> inputReadLines)
    {
        var allLines = new List<List<int>>();
        
        foreach (var line in inputReadLines)
        {
            var newLine = new List<int>();
            var splitLine = line.Split(' ');

            newLine.AddRange(splitLine.Select(int.Parse));
            allLines.Add(newLine);
        }

        return allLines;
    }

    private static int GetNextValue(List<int> readLine)
    {
        var sequences = BuildAllSequences(readLine);
        var nextValue = CalculateNextValue(sequences);

        return nextValue;
    }
        
    private static int GetBackwardsValue(List<int> readLine)
    {
        var reversed = new List<int>(readLine);
        reversed.Reverse();
        
        var sequences = BuildAllSequences(reversed);
        var nextValue = CalculateNextValue(sequences);

        return nextValue;
    }

    private static List<List<int>> BuildAllSequences(List<int> readLine)
    {
        var sequences = new List<List<int>>();
        var currentSequence = new List<int>(readLine);
        List<int> nextSequence; 
        
        sequences.Add(readLine);
        
        do
        {
            nextSequence = new List<int>();
            
            for (var i = 0; i < currentSequence.Count - 1; i++)
            {
                var number = currentSequence[i];
                var result = currentSequence[i + 1] - number;
            
                nextSequence.Add(result);
            }
            sequences.Add(nextSequence);
            currentSequence = new List<int>(nextSequence);
        } while (!IsAllZeroes(nextSequence));

        return sequences;
    }

    private static bool IsAllZeroes(List<int> nextSequence)
    {
        return nextSequence.All(number => number.Equals(0));
    }

    private static int CalculateNextValue(List<List<int>> sequences)
    {
        var totalSteps = sequences.Count - 1;
        var nextValue = sequences[totalSteps].LastOrDefault();
        
        for (var i = totalSteps; i > 0 ; i--)
        {
            var upsideLineLastNumber = sequences[i - 1].LastOrDefault();
            nextValue = upsideLineLastNumber + nextValue;
        }

        return nextValue;
    }
}