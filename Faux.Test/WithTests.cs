using System.Reflection;
using Faux.Lib;
using Faux.Lib.Generators;
using Faux.Test.Models;

namespace Faux.Test;

[TestFixture]
public class WithTests
{
    public Faux<FakeModel> faux;
    public FakeModel model;

    [SetUp]
    public void Init()
    {
        model = new FakeModel();
        faux = new Faux<FakeModel>(model);
    }

    [Test]
    public void TestWith()
    {
        var genFaux = faux.With<string>(m => m.Name, model.NewString, Guid.NewGuid().ToString())
            .StringLength(15).GenerateMultiple();
    }

    [Test]
    public void TestRandomDouble()
    {
        var random = NumberGenerator.GenerateDouble(1, 1000);
        var t = 0;
    }
}