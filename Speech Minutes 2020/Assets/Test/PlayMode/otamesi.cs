using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

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
            Assert.AreEqual(1, i);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator otamesiWithEnumeratorPasses()
        {
            var boad = new GameObject("drowingboad");
           // var rec = boad.GetComponent<RectTransform>();
            var paint = boad.AddComponent<Painter>();
            //Assert.IsFalse(paint.mode);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
           // Assert.IsTrue(paint.mode);
        }
    }
}
