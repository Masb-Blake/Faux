using System.Collections.Concurrent;
using Faux.Test.Helpers;

namespace Faux.Test.Collections;

[TestFixture]
public class DynamicConcurrentQueueTests
{

    [Test]
    public void CCQueueStringTest()
    {
        var faux = FauxHelper.GenerateFaux<ConcurrentQueueString>();
        Assert.AreEqual(faux.Count(), 1);
        Assert.AreEqual(faux.FirstOrDefault().Queue.Count(), 5);
        
                    
    }
    
}

public class ConcurrentQueueString()
{
    public ConcurrentQueue<string> Queue { get; set; }
}
