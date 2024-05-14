using Faux.Lib;

namespace Faux.Test.Collections;

[TestFixture]
public class DynamicListsTests
{
    [Test]
    public void TestStringList()
    {
        var sList = new StringList();
        var faux = new Faux<StringList>(sList).GenerateMultiple();
        var stringLists = faux as StringList[] ?? faux.ToArray();
        Assert.AreEqual(stringLists.Length, 1);
        Assert.AreEqual(stringLists.FirstOrDefault().List.Count, 5);
    }


    [Test]
    public void TestObjectList()
    {
        var oList = new ObjectList();
        var faux = new Faux<ObjectList>(oList).GenerateMultiple();
        var objectList = faux as ObjectList[] ?? faux.ToArray();
        Assert.AreEqual(objectList.FirstOrDefault().List.Count, 5);
    }
}


public class StringList
{
    public List<string> List { get; set; }
}

public class IntList
{
    public List<int> List { get; set; }
}

public class DecimalList
{
    public List<decimal> List { get; set; }
}

public class BoolList
{
    public List<bool> List { get; set; }
}

public class DateList
{
    public List<DateTime> List { get; set; }
}

public class ObjectList
{
    public List<MockObject> List { get; set; }
} 
