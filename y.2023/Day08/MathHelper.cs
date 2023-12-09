namespace Day08;

public static class MathHelper
{
    public static long CalculateLcm(long[] numbers)
    {
        var lcm = numbers[0];
        for (var i = 1; i < numbers.Length; i++)
        {
            lcm = CalculateLcm(lcm, numbers[i]);
        }

        return lcm;
    }

    public static long CalculateLcm(long a, long b)
    {
        return Math.Abs(a * b) / CalculateGcd(a, b);
    }

    public static long CalculateGcd(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}