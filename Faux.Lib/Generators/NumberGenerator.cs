namespace Faux.Lib.Generators;

public static class NumberGenerator
{
   public static byte GenerateByte(byte min = byte.MinValue, byte max = byte.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToByte(Random(vals[0], vals[1]));
   }

   public static sbyte GenerateSByte(sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToSByte(Random(vals[0], vals[1]));
   }
   
   public static short GenerateShort(short min = short.MinValue, short max = short.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToInt16(Random(vals[0], vals[1]));
   }

   public static ushort GenerateUShort(ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToUInt16(Random(vals[0], vals[1]));
   }

   public static int GenerateInt(int min = int.MinValue, int max = int.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToInt32(Random(vals[0], vals[1]));
   }

   public static uint GenerateUInt(uint min = uint.MinValue, uint max = uint.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Convert.ToUInt32(Random(vals[0], vals[1]));
   }

   public static long GenerateLong(long min = long.MinValue, long max = long.MaxValue)
   {
      var vals = CheckValues(min, max);
      return Random(vals[0], vals[1]);
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