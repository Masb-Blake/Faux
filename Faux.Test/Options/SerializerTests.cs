using Faux.Lib;
using Faux.Test.Collections;

namespace Faux.Test.Options;

[TestFixture]
public class SerializerTests
{
    [Test]
    public void JsonSerializationTest()
    {
        var jsObj = new JsonSerialObject();
        var faux = new Faux<JsonSerialObject>(jsObj);
        var json = faux.GenerateMultiple(2).ToJson();
        Assert.That(json, Is.Not.Null);
    }
}

public class JsonSerialObject()
{
    
    public string Name { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public List<string> List { get; set; }
    public Dictionary<string, MockObject> Objects { get; set; }
    public List<int> Values { get; set; }
    public Stack<decimal> Dollars { get; set; }
}