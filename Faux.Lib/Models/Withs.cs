namespace Faux.Lib.Models;

public class Withs : FauxPropertyInfo
{
    public Delegate Function { get; set; }
    public object[] Args { get; set; }
}