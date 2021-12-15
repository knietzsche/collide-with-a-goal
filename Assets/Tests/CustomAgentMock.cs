using UnityEngineProvider;

public class CustomAgentMock : CustomAgent
{
    private float rewardTotal;

    public new void StartRoutine()
    {
        base.StartRoutine();
        base.OnEpisodeBegin();
    }

    public void SetInput(IInput input)
    {
        this.input = input;
    }

    public void SetTime(ITime time)
    {
        this.time = time;
    }

    public override void OnEpisodeBegin() { }

    protected override void AddRewardRoutine(float increment)
    {
        rewardTotal += increment;

        base.AddRewardRoutine(increment);
    }

    public float GetTotalReward()
    {
        return rewardTotal;
    }
}

