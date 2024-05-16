using Faux.Lib;
using Faux.Lib.Exceptions;
using Faux.Test.Collections;

namespace Faux.Test.Options;

[TestFixture]
public class SetTests
{
    [Test]
    public void TestSetsString()
    {
        var setClass = new SetClass();
        var faux = new Faux<SetClass>(setClass);
        var generated = faux.Set(c => c.Name, "TestName")
            .Set(c => c.Last, "TestLast")
            .GenerateList().FirstOrDefault();
        Assert.That(generated, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(generated.Name, Is.EqualTo("TestName"));
            Assert.That(generated.Last, Is.EqualTo("TestLast"));
        });
    }

    [Test]
    public void TestSetsObject()
    {
        var setObj = new SetClassObject();
        var faux = new Faux<SetClassObject>(setObj);
        var generated = faux.Set(c => c.Object, new MockObject()
        {
            Age = 10,
            DateOfBirth = DateTime.Now,
            IsAdult = false,
            Name = "Test"
        }).GenerateList().FirstOrDefault();
        Assert.That(generated, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(generated.Object.Age, Is.EqualTo(10));
            Assert.That(generated.Object.IsAdult, Is.EqualTo(false));
            Assert.That(generated.Object.Name, Is.EqualTo("Test"));
        });
    }

    [Test]
    public void TestMultipleSets()
    {
        var setClass = new SetClass();
        var faux = new Faux<SetClass>(setClass);
        var generated = faux.Set(n => new[] {n.Last, n.Name}, "Test")
            .GenerateList()
            .FirstOrDefault();
        Assert.That(generated, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(generated.Last, Is.EqualTo("Test"));
            Assert.That(generated.Name, Is.EqualTo("Test"));
        });
        
    }
    
    [Test]
    public void TestSetDuplicateStatementException()
    {
        var setClass = new SetClass();
        var faux = new Faux<SetClass>(setClass);
        Assert.Throws<DuplicateStatementException>(() => faux.Set(c => c.Name, "Test")
            .Set(c => c.Last, "Last")
            .With(c => c.Name, () => "LOL", null)
            .GenerateMultiple());
    }
}

public class SetClass()
{
    public string Name { get; set; }
    public string Last { get; set; }
}

public class SetClassObject()
{
    public string Name { get; set; }
    public MockObject Object { get; set; }
 }