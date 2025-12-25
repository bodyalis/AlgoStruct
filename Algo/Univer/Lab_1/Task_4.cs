namespace Algo.Univer.Lab_1;

public class Task_4
{
    public class Interval
    {
        public int Min_Excluded { get; set; }
        public int Max_Included { get; set; }
    }

    
    // [1,4), [2,5), [5,6), [7,9)
    private static readonly List<Interval> _intervals =
    [
        new() { Min_Excluded = 1, Max_Included = 4 },
        new() { Min_Excluded = 2, Max_Included = 5 },
        new() { Min_Excluded = 5, Max_Included = 6 },
        new() { Min_Excluded = 7, Max_Included = 9 },
    ];
    
    public List<int> Solve()
    {
        IOrderedEnumerable<Interval> sortedByMaxIncludedIntervals = _intervals.OrderBy(i => i.Max_Included);

        List<int> result = new List<int>();
        int prevPoint = int.MinValue;
        foreach (Interval interval in _intervals)
        {
            if (interval.Min_Excluded > prevPoint)
            {
                 result.Add(interval.Max_Included);
                prevPoint = interval.Max_Included;
            }
        }

        return result;
    }
}