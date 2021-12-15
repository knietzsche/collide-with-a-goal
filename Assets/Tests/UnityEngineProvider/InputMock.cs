using UnityEngine;

namespace UnityEngineProvider
{
    public class InputMock : IInput
    {
        private int index;
        private KeyCode[] values;
        private int counter;

        public InputMock(KeyCode[] values)
        {
            this.values = values;
        }

        public bool GetKey(KeyCode keyCode)
        {
            if (keyCode == values[index])
            {
                if (++index >= values.Length) index = 0;
                counter++;

                return true;
            }

            return false;
        }

        public int GetCounter()
        {
            return counter;
        }
    }
}
