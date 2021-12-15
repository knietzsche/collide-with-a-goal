namespace UnityEngineProvider
{
    public interface IRandom<T>
    {
        public T Generate();
    }

    class RandomInt : IRandom<int>
    {
        int minInclusive;
        int maxExclusive;
        int offset;

        public RandomInt(int minInclusive, int maxExclusive, int offset)
        {
            this.minInclusive = minInclusive;
            this.maxExclusive = maxExclusive;
            this.offset = offset;
        }

        public int Generate()
        {
            return UnityEngine.Random.Range(minInclusive, maxExclusive) + offset;
        }
    }

    class RandomFloat : IRandom<float>
    {
        float minInclusive;
        float maxInclusive;

        public RandomFloat(float rangeInclusive)
        {
            minInclusive = rangeInclusive > 0 ? -rangeInclusive : rangeInclusive;
            maxInclusive = rangeInclusive > 0 ? rangeInclusive : -rangeInclusive;
        }

        public float Generate()
        {
            return UnityEngine.Random.Range(minInclusive, maxInclusive);
        }
    }
}
