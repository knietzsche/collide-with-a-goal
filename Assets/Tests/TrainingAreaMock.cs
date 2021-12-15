using UnityEngineProvider;

public class TrainingAreaMock : TrainingArea
{
    public new void StartRoutine()
    {
        base.StartRoutine();
    }

    public void SetGoalCount(int goalCount)
    {
        this.goalCount = goalCount;
    }

    public void SetUnityObject(IObject  unityObject)
    {
        this.unityObject = unityObject;
    }

    public void SetRandomAgentPlacement(bool value)
    {
        randomAgentPlacement = value;
    }

    public void SetRandomSpawnRange(IRandom<float> randomSpawnRange)
    {
        this.randomSpawnRange = randomSpawnRange;
    }

    public void SetRandomGoalCount(IRandom<int> randomGoalCount)
    {
        this.randomGoalCount = randomGoalCount;
    }
}
