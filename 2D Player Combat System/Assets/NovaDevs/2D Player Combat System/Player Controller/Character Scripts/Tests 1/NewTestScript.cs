using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{

    /// <summary>
    /// Should return max speed value Tests
    /// </summary>
    [Test]
    public void ShouldReturnMaxSpeedValue()
    {
        // Use the Assert class to test conditions

        var value = 0f;

        if (value < 2f)
        {
            value += 0.001f;

            if (value >= 2f)
            {
                value = 2f;
                Assert.AreEqual(value, 2f);
            }
        }
    }

    [UnityTest]
    public IEnumerator ShouldReturnMaxSpeedValue2()
    {
        // Use the Assert class to test conditions

        var value = 0f;

        if (value < 2f)
        {
            value += 0.001f;

            if (value >= 2f)
            {
                value = 2f;
                Assert.AreEqual(value, 2f);
            }
        }
        yield return new WaitForSeconds(.1f);


    }


    /// <summary>
    /// Should return max speed value Tests
    /// </summary>
    [Test]
    public void ShouldReturnGroundedValue()
    {
        // Use the Assert class to test conditions

        var value = 0f;

        if (value < 2f)
        {
            value += 0.001f;

            if (value >= 2f)
            {
                value = 2f;
                Assert.AreEqual(value, 2f);
            }
        }
    }

    [UnityTest]
    public IEnumerator ShouldReturnGroundedValue1()
    {
        // Use the Assert class to test conditions

        var value = 0f;

        if (value < 2f)
        {
            value += 0.001f;

            if (value >= 2f)
            {
                value = 2f;
                Assert.AreEqual(value, 2f);
            }
        }
        yield return new WaitForSeconds(.1f);

    }
}
