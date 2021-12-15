using NUnit.Framework;

namespace UnityEngineProvider
{
    namespace MockTest
    {
        class RandomMock
        {
            [Test]
            public void Generate()
            {
                int[] values = new int[] { 1, 1, 2, 3, 5, 8, 13 };

                var random = new RandomMock<int>(values);

                for (int i = 0; i < values.Length; i++)
                {
                    Assert.True(random.Generate() == values[i]);
                }

                for (int i = 0; i < values.Length; i++)
                {
                    Assert.True(random.Generate() == values[i]);
                }
            }
        }
    }
}
