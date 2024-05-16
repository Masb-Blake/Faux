using System.Runtime.InteropServices;

namespace Faux.Lib.Models;

public class FauxOptions
{

    public int StringLength { get; set; } = 10;
    public int InnerListLength { get; set; } = 5;
    public int[] IntRange { get; set; } = [int.MaxValue, int.MinValue];
    public bool MultiThreaded { get; set; } = false;
    
}