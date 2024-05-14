namespace Faux.Lib.Generators;

public static class BoolGenerator
{
    public static bool Generate()
    {
        return Random.Shared.Next(2) == 1;
    }
}