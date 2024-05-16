using System.Collections;
using Faux.Lib;
using Helper = Faux.Test.Helpers.FauxHelper;

namespace Faux.Test.Collections;

[TestFixture]
public class DynamicListsTests
{
    [Test]
    public void TestStringList()
    {
        var faux = Helper.GenerateFaux<StringList>();
        Assert.AreEqual(faux.Count(), 1);
        Assert.AreEqual(faux.FirstOrDefault().List.Count, 5);

        var checkType = faux.FirstOrDefault().List.FirstOrDefault();
        Assert.AreEqual(checkType.GetType(), typeof(string));
    }


    [Test]
    public void TestObjectList()
    {
        var faux = Helper.GenerateFaux<ObjectList>();
        Assert.AreEqual(faux.FirstOrDefault().List.Count, 5);

        var checkType = faux.FirstOrDefault().List.FirstOrDefault();
        Assert.AreEqual(checkType.GetType(), typeof(MockObject));
    }

    [Test]
    public void TestIntList()
    {
        var faux = Helper.GenerateFaux<IntList>();
        Assert.AreEqual(faux.FirstOrDefault().List.Count, 5);

        var checkType = faux.FirstOrDefault().List.FirstOrDefault();
        Assert.AreEqual(checkType.GetType(), typeof(int));
    }

    [Test]
    public void TestDecimalList()
    {
        var faux = Helper.GenerateFaux<DecimalList>();
        Assert.AreEqual(faux.FirstOrDefault().List.Count, 5);

        var checkType = faux.FirstOrDefault().List.FirstOrDefault();
        Assert.AreEqual(checkType.GetType(), typeof(decimal));
    }

    [Test]
    public void TestBoolList()
    {
        var faux = Helper.GenerateFaux<BoolList>();
        Assert.AreEqual(faux.FirstOrDefault().List.Count, 5);

        var checkType = faux.FirstOrDefault().List.FirstOrDefault();
        Assert.AreEqual(checkType.GetType(), typeof(bool));
    }

    [Test]
    public void TestDateTimeList()
    {
        var faux = Helper.GenerateFaux<DateList>();
        Assert.AreEqual(faux.FirstOrDefault().List.Count, 5);

        var checkType = faux.FirstOrDefault().List.FirstOrDefault();
        Assert.AreEqual(checkType.GetType(), typeof(DateTime));
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
