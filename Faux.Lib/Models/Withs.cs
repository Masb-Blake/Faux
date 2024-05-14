namespace Faux.Lib.Models;

public class Withs
{
    public string PropertyName { get; set; }
    public Delegate Function { get; set; }
    public object[] Args { get; set; }
}