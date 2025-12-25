namespace Algo.Univer.Lab_1;

public class Task_3
{
    public class Interval
    {
        public int StartedAt { get; set; }
        public int EndedAt { get; set; }
    }

    private static readonly List<Interval> _intervals =
    [
        new() { StartedAt = 1, EndedAt = 3 },
        new() { StartedAt = 2, EndedAt = 5 },
        new() { StartedAt = 4, EndedAt = 6 },
        new() { StartedAt = 6, EndedAt = 7 }
    ];
    
    public List<Interval> Solve()
    {
        var sortedByEndTimeIntervals = _intervals.OrderBy(s => s.EndedAt);
        List<Interval> result = new List<Interval>();

        int prevEnd = int.MinValue;
        foreach (Interval interval in sortedByEndTimeIntervals)
        {
            if (interval.StartedAt >= prevEnd)
            {
                result.Add(interval);
                prevEnd = interval.EndedAt;
            }
        }

        return result;
    }
}