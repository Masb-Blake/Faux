namespace Faux.Lib.Generators;

public static class StringGenerator
{
   public static string Generate(int stringLength)
   {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(letters, stringLength).Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
   }
}