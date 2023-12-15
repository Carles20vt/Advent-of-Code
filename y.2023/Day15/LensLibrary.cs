namespace Day15;

public class LensLibrary
{
    private readonly List<string> initializationSequence;
    private static readonly object Mutex = new();

    private List<List<(string, int)>> boxes;
    
    public LensLibrary(List<string> inputLines)
    {
        initializationSequence = inputLines[0].Split(',').ToArray().ToList();
        boxes = new List<List<(string, int)>>(256);

        for (var i = 0; i < 256; i++)
        {
            boxes.Add(new List<(string, int)>());
        }
    }

    public long CalculateTotalSumOfHash()
    {
        long sumOfHash = 0;
        
        Parallel.ForEach(initializationSequence, (step) =>
        {
            var stepHash = CalculateHash(step);
            
            lock (Mutex)
            {
                sumOfHash += stepHash;
            }
        });

        return sumOfHash;
    }

    public long CalculateFocusingPower()
    {
        var totalFocusingPower = 0;

        PutLensesOnBoxes();

        foreach (var box in boxes)
        {
            if (!box.Any())
            {
                continue;
            }

            var boxSlotNumber = boxes.IndexOf(box) + 1;

            var totalSlots = box.Count;
            var slotNumber = 1;
            
            for (var currentFocalLent = totalSlots; currentFocalLent > 0; currentFocalLent--)
            {
                var amount = boxSlotNumber * slotNumber * box[currentFocalLent - 1].Item2;
                totalFocusingPower += amount;
                slotNumber++;
            }
        }

        return totalFocusingPower;
    }

    private void PutLensesOnBoxes()
    {
        foreach (var step in initializationSequence)
        {
            var isDashOperation = step.Contains('-');
            if (isDashOperation)
            {
                DoDashOperation(step);
            }
            
            var isEqualsOperation = step.Contains('=');
            if (isEqualsOperation)
            {
                DoEqualsOperation(step);
            }
        }
    }

    private void DoDashOperation(string step)
    {
        var splitStep = step.Split('-');
        var label = splitStep[0];
        var boxNumber = CalculateHash(label);

        var isLabelPresent = boxes[boxNumber].Any(x => x.Item1.Equals(label));
        if (!isLabelPresent)
        {
            return;
        }

        boxes[boxNumber].RemoveAll(x => x.Item1.Equals(label));
    }

    private void DoEqualsOperation(string step)
    {
        var splitStep = step.Split('=');
        var label = splitStep[0];
        var focalLength = int.Parse(splitStep[1].Trim());
        var boxNumber = CalculateHash(label);

        var isLabelPresent = boxes[boxNumber].Any(x => x.Item1.Equals(label));
        if (isLabelPresent)
        {
            var oldLens = boxes[boxNumber].FirstOrDefault(x => x.Item1.Equals(label));
            var oldLensIndex= boxes[boxNumber].IndexOf(oldLens);
            
            boxes[boxNumber].Remove(oldLens);
            boxes[boxNumber].Insert(oldLensIndex, (label, focalLength));
            
            return;
        }

        boxes[boxNumber].Insert(0, (label, focalLength));
    }

    private static int CalculateHash(string step)
    {
        var stepSplit = step.ToCharArray();
        var currentValue = 0;

        foreach (var currentChar in stepSplit)
        {
            currentValue += Convert.ToInt32(currentChar);
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }
}