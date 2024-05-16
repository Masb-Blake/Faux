namespace Faux.Lib.Generators;

public static class DecimalGenerator
{
   public static decimal Generate(int maxScale = 29)
   {
      var scale = (byte)Random.Shared.Next(maxScale);
      var sign = Random.Shared.Next(2) == 1;
      return new decimal(NextInt(), NextInt(), NextInt(), sign, scale);
   }

   private static int NextInt()
   {
      var first = Random.Shared.Next(0, 1 << 4) << 28;
      var last = Random.Shared.Next(0, 1 << 28);
      return first | last;
   }
}