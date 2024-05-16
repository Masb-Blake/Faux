namespace Faux.Lib.Models;

public class With : FauxPropertyInfo
{
    public Delegate Function { get; set; }
    public object[] Args { get; set; }
}