namespace UnityEngineProvider
{
    public interface ITime
    {
        float GetFixedDeltaTime();
    }

    public class Time : ITime
    {
        private float coefficient;

        public Time(float coefficient = 1f)
        {
            this.coefficient = coefficient;
        }

        public float GetFixedDeltaTime()
        {
            return UnityEngine.Time.fixedDeltaTime * coefficient;
        }
    }
}
