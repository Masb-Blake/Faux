namespace Faux.Lib.Generators;

public static class NumberGenerator
{
   public static byte GenerateByte()
   {
      return Convert.ToByte(Random(byte.MinValue, byte.MinValue));
   }

   public static sbyte GenerateSByte()
   {
      return Convert.ToSByte(Random(sbyte.MinValue, sbyte.MaxValue));
   }
   
   public static short GenerateShort()
   {
      return Convert.ToInt16(Random(short.MinValue, short.MaxValue));
   }

   public static ushort GenerateUShort()
   {
      return Convert.ToUInt16(Random(0, 65536));
   }

   public static int GenerateInt(int min = int.MinValue, int max = int.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToInt32(Random(vals[0], vals[1]));
   }

   public static uint GenerateUInt()
   {
      return Convert.ToUInt32(Random(uint.MinValue, uint.MaxValue));
   }

   public static long GenerateLong()
   {
      return Random(long.MinValue, long.MaxValue);
   }

   public static double GenerateDouble(double start = double.MinValue, double end = double.MaxValue)
   {
      var rand = System.Random.Shared.NextDouble();
      return (end * rand) + (start * (1d - rand));
   }

   public static float GenerateSingle(float start = float.MinValue, float end = float.MaxValue)
   {
      var rand = System.Random.Shared.NextSingle();
      return (end * rand) + (start * (1f - rand));
   }
   
   private static long Random(long start, long end)
   {
      return System.Random.Shared.NextInt64(start, end);
   }

   private static long[] CheckValues(ValueType min, ValueType max)
   {
      var convertedMin = Convert.ToInt64(min);
      var convertedMax = Convert.ToInt64(max);
      if (convertedMin <= convertedMax) return [convertedMin, convertedMax];
      convertedMin = convertedMax;
      convertedMax = Convert.ToInt64(min);
      return [convertedMin, convertedMax];
   }
}