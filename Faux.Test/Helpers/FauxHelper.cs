using Faux.Lib;
namespace Faux.Test.Helpers;

public static class FauxHelper
{
     public static IEnumerable<T> GenerateFaux<T>() where T : new()
    {
        return new Faux<T>(new T()).GenerateList();
    }
}