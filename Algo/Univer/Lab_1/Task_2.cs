namespace HuffmanAlgorithm.Univer.Lab_1;

public class Task_2
{
    public record Coin(int Value);


    private readonly List<Coin> _coins = new List<Coin>()
    {
        new(25),
        new(10),
        new(5),
        new(1),
    };

    private const int MaxSum = 99;

    public List<(Coin coin, int count)> Solve()
    {
        var sortedCoins = _coins.OrderByDescending(c => c.Value);

        int remainingSum = MaxSum;
        List<(Coin, int)> result = new();
        foreach (Coin coin in sortedCoins)
        {
            int count = remainingSum / coin.Value;
            if (count > 0)
            {
                result.Add((coin, count));
                remainingSum -= coin.Value * count;
            }
        }

        if (remainingSum > 0)
        {
            throw new Exception("Невозможно решить с помощью жадного алгоритма");
        }

        return result;
    }
}