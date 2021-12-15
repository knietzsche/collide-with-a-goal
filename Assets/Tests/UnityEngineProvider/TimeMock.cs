namespace UnityEngineProvider
{
    public class TimeMock : ITime
    {
        private const float fixedDeltaTimeDefault = .02f;

        private float coefficient;
        private float fixedDeltaTime;

        public TimeMock(float coefficient = 1f, float fixedDeltaTime = fixedDeltaTimeDefault)
        {
            this.fixedDeltaTime = fixedDeltaTime;
            this.coefficient = coefficient;
        }

        public float GetFixedDeltaTime()
        {
            return fixedDeltaTime * coefficient;
        }
    }
}
