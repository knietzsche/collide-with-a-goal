using System;
using UnityEngine;

namespace UnityEngineProvider
{
    public class ObjectMock : IObject
    {
        private string tag;

        public ObjectMock(string tag)
        {
            this.tag = tag;
        }

        public T Instantiate<T>(T prefab, Transform transform) where T : UnityEngine.Object
        {
            if (prefab != null) throw new ArgumentException();
            
            var gameObject = new GameObject();
            gameObject.transform.parent = transform;
            gameObject.AddComponent(typeof(T));
            gameObject.AddComponent<Rigidbody>();
            gameObject.tag = tag;

            return gameObject.GetComponent<T>();
        }
    }
}
