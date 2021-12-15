using UnityEngine;

namespace UnityEngineProvider
{
    public interface IInput
    {
        public bool GetKey(KeyCode keyCode);
    }

    class Input : IInput
    {
        public bool GetKey(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }
    }
}

