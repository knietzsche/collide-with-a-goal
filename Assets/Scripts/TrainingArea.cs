using UnityEngineProvider;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainingArea : MonoBehaviour
{
    public const float DistanceMinAgentGoal = 1.5f;
    public const float DistanceMinGoalGoal = 1f;
    public const float LocalPositionAgentY = 1f;
    public const float LocalPositionGoalY = .5f;

    public const float RewardGoalCollected = 1f;
    public const float RewardObstacleCollected = -1f;
    public const float RewardEndEpisode = 3f;
    public const float RewardOnActionReceived = -.01f;

    private const float spawnRange = 9.5f;
    private const int iterationCap = short.MaxValue;

    [SerializeField] [Min(1)] private int randomGoalCountMaxInclusive = 100;
    [SerializeField] protected Goal prefabGoal = null;
    [SerializeField] [Min(0)] [Tooltip("0 = random")] protected int goalCount = 1;
    [SerializeField] protected Goal prefabObstacle = null;
    [SerializeField] [Range(0f, .99f)] protected float obstacleRatio = 0f;
    [SerializeField] protected bool randomAgentPlacement = false;

    protected IObject unityObject;
    protected IRandom<int> randomGoalCount;
    protected IRandom<float> randomSpawnRange;
    protected IRandom<float> rotationRange;

    private CustomAgent agent;
    private List<Goal> goalList = new List<Goal>();

    private void Start()
    {
        StartRoutine();
    }

    protected void StartRoutine()
    {
        unityObject ??= new UnityEngineProvider.Object();
        randomGoalCount ??= new RandomInt(0, randomGoalCountMaxInclusive, 1);
        randomSpawnRange ??= new RandomFloat(spawnRange);
        rotationRange ??= new RandomFloat(90f);
        agent ??= GetComponentInChildren<CustomAgent>();
    }

    private void Update()
    {
        DrawDebugLines();
    }

    public virtual void ResetEpisode()
    {
        Clear();
        ResetAgent();
        SpawnGoals();
    }

    private void ResetAgent()
    {
        agent.transform.localPosition = randomAgentPlacement ?
            GenerateSpawnLocalPosition(LocalPositionAgentY) : new Vector3(0f, LocalPositionAgentY, 0f);
        agent.transform.localRotation = randomAgentPlacement ?
            Quaternion.Euler(0f, rotationRange.Generate(), 0f) : Quaternion.identity;
        agent.StopMovement();
    }

    private void SpawnGoals()
    {
        var count = goalCount > 0 ? goalCount : randomGoalCount.Generate();
        var obstacleCount = (int) Math.Min(count * obstacleRatio, count - 1);
        count -= obstacleCount;

        for (var i = 0; i < count; i++)
        {
            try { SpawnGoal(prefabGoal); } catch (ApplicationException) { break; }
        }
        for (var i = 0; i < obstacleCount; i++)
        {
            try { SpawnGoal(prefabObstacle); } catch (ApplicationException) { break; }
        }
    }

    private void SpawnGoal(Goal prefab)
    {
        var localPosition = GenerateSpawnLocalPosition(LocalPositionGoalY);

        var goal = unityObject.Instantiate(prefab, transform);
        goal.transform.localPosition = localPosition;

        goalList.Add(goal);
    }

    private void Clear()
    {
        foreach (var goal in goalList)
        {
            Destroy(goal.gameObject);
        }
        goalList.Clear();
    }

    private Vector3 GenerateSpawnLocalPosition(float y)
    {
        Vector3 localPosition;
        int counter = 0;

        do
        {
            localPosition = new Vector3(randomSpawnRange.Generate(), y, randomSpawnRange.Generate());
            if (++counter >= iterationCap) throw new ApplicationException();
        } while (IsOccupied(localPosition));

        return localPosition;
    }

    public void Collect(Goal goal)
    {
        if (goalList.Remove(goal))
        {
            Destroy(goal.gameObject);

            agent.AddReward(goal.CompareTag(Goal.TagDefault) ?
                RewardGoalCollected :
                RewardObstacleCollected);

            bool pass = true;
            foreach(var g in goalList)
            {
                if (g.CompareTag(Goal.TagDefault))
                {
                    pass = false;
                    break;
                }
            }

            if (pass)
            {
                agent.AddReward(RewardEndEpisode);
                agent.EndEpisode();
            }
        }
    }

    private bool IsOccupied(Vector3 localPosition)
    {
        if (Vector3.Distance(agent.transform.localPosition, localPosition) < DistanceMinAgentGoal) return true;

        foreach (var goal in goalList)
        {
            if (Vector3.Distance(goal.transform.localPosition, localPosition) < DistanceMinGoalGoal) return true;
        }

        return false;
    }

    private void DrawDebugLines()
    {
        float distanceMin = float.MaxValue;
        Vector3? positionDistanceMin = null;

        foreach (var goal in goalList)
        {
            if (!goal.CompareTag(Goal.TagDefault)) continue;

            var distance = Vector3.Distance(goal.transform.localPosition, agent.transform.localPosition);

            if (distance < distanceMin)
            {
                distanceMin = distance;
                positionDistanceMin = goal.transform.position;
            }
        }

        if (positionDistanceMin.HasValue)
        {
            Debug.DrawLine(agent.transform.position, positionDistanceMin.Value, Color.green);
        }
    }
}
