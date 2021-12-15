using UnityEngine;

namespace UnityEngineProvider
{
    public interface IObject
    {
        public T Instantiate<T>(T prefab, Transform transform) where T : UnityEngine.Object;
    }

    class Object : IObject
    {
        public T Instantiate<T>(T prefab, Transform transform) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(prefab, transform);
        }
    }
}
