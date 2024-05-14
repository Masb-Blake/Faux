namespace Faux.Lib.Generators;

public static class DateTimeGenerator
{
    public static DateTime Generate(DateTime start, DateTime end)
    {
        var begin = start;
        var last = end;
        if (start > end)
        {
            last = start;
            begin = end;
        }
        var randomRange = Random.Shared.Next(0, last.Subtract(begin).Days + 1);
        return begin.AddDays(randomRange);
    }
}