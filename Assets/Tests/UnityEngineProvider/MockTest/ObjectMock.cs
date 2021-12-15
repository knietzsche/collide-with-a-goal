using NUnit.Framework;
using UnityEngine;

namespace UnityEngineProvider
{
    namespace MockTest
    {
        class MonoBehaviourDummy : MonoBehaviour { }

        class ObjectMock
        {
            [Test]
            public void Instantiate()
            {
                var tagMock = "Untagged";
                var objectMock = new UnityEngineProvider.ObjectMock(tagMock);
                var parent = new GameObject();

                var instance = objectMock.Instantiate((MonoBehaviourDummy)null, parent.transform);
                Assert.True(instance.GetType() == typeof(MonoBehaviourDummy));
                Assert.True(instance.transform.parent == parent.transform);
                Assert.True(instance.CompareTag(tagMock));
            }
        }
    }
}
