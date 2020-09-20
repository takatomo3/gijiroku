using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class otamesi
    {
        // A Test behaves as an ordinary method
        [Test]
        public void otamesiSimplePasses()
        {
            // Use the Assert class to test conditions
            int i = 1;
            Assert.AreEqual(-2, i);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator otamesiWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
