using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BasicTest
{
    [SetUp]
    public void BasicTestSetup()
    {
        //Run setup here.
    }

    // A Test behaves as an ordinary method
    [Test]
    public void BasicTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(1f, 1f);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BasicTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
