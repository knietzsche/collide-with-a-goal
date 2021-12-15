namespace UnityEngineProvider
{
    class RandomMock<T> : IRandom<T>
    {
        int index;
        T[] values;

        public RandomMock(T[] values)
        {
            this.values = values;
        }

        public T Generate()
        {
            var value = values[index];

            if (++index >= values.Length) index = 0;

            return value;
        }
    }
}