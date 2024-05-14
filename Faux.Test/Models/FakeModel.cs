namespace Faux.Test.Models;

public class FakeModel
{
    public FakeModel()
    {
        
    }

    public string Name { get; set; } = "Hello";
    public int Integer { get; set; } = 0;
    public HashSet<string> TestEnum { get; set; }
    public Dictionary<string, string> TestDict { get; set; }
    public FakeInner Inner
    {
        get;
        set;
    }

    public string NewString(string uuid) => uuid;
    public int NewInt => 1;
}