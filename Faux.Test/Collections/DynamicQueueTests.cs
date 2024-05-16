using Faux.Test.Helpers;

namespace Faux.Test.Collections;

[TestFixture]
public class DynamicQueueTests
{

    [Test]
    public void TestStringQueue()
    {
        var faux = FauxHelper.GenerateFaux<QueueString>();
        Assert.AreEqual(faux.Count(), 1);
        Assert.AreEqual(faux.FirstOrDefault().Queue.Count, 5);

        var checkType = faux.FirstOrDefault().Queue.FirstOrDefault().GetType();
        Assert.AreEqual(checkType, typeof(string));
    }

    [Test]
    public void TestObjectQueue()
    {
        var faux = FauxHelper.GenerateFaux<QueueObject>();
        Assert.AreEqual(faux.Count(), 1);
        Assert.AreEqual(faux.FirstOrDefault().Queue.Count, 5);

        var checkType = faux.FirstOrDefault().Queue.FirstOrDefault().GetType();
        Assert.AreEqual(checkType, typeof(MockObject));
    }

    [Test]
    public void TestIntQueue()
    {
        var faux = FauxHelper.GenerateFaux<QueueInt>();
        Assert.AreEqual(faux.Count(), 1);
        Assert.AreEqual(faux.FirstOrDefault().Queue.Count(), 5);

        var checkType = faux.FirstOrDefault().Queue.FirstOrDefault().GetType();
        Assert.AreEqual(checkType, typeof(int));
    }
    
}


public class QueueString()
{
    public Queue<string> Queue { get; set; }
}

public class QueueObject()
{
    public Queue<MockObject> Queue { get; set; }
}

public class QueueInt()
{
    public Queue<int> Queue { get; set; }
}