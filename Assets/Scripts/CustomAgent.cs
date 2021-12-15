using UnityEngineProvider;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CustomAgent : Agent
{
    public const int VectorActionSize = 2;
    public const int VectorObservationSize = 1;
    public const int HeuristicMovementIndex = 0;
    public const float HeuristicMovementUp = 1f;
    public const float HeuristicMovementDown = -1f;
    public const int HeuristicRotationIndex = 1;
    public const float HeuristicRotationRight = 1f;
    public const float HeuristicRotationLeft = -1f;
    public const float RotationTimeCoefficient = 200f;
    
    protected IInput input;
    protected ITime time;

    private Rigidbody body;
    private TrainingArea trainingArea;

    private int episodeStart;

    public override void Initialize()
    {
        body = GetComponent<Rigidbody>();
        trainingArea = GetComponentInParent<TrainingArea>();
    }

    private void Start()
    {
        StartRoutine();
    }

    protected void StartRoutine()
    {
        input ??= new UnityEngineProvider.Input();
        time ??= new UnityEngineProvider.Time(RotationTimeCoefficient);
    }

    public override void OnEpisodeBegin()
    {
        if (body != null) body.velocity = Vector3.zero;
        episodeStart = Academy.Instance.StepCount;
        trainingArea?.ResetEpisode();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Magnitude(body.velocity));
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        body.AddForce(transform.forward * vectorAction[0], ForceMode.VelocityChange);
        transform.Rotate(transform.up * vectorAction[1], time.GetFixedDeltaTime());

        AddReward(TrainingArea.RewardOnActionReceived);
    }

    public new void AddReward(float increment)
    {
        AddRewardRoutine(increment);
    }

    protected virtual void AddRewardRoutine(float increment)
    {
        base.AddReward(increment);
    }

    public void StopMovement()
    {
        if (body != null) body.velocity = Vector3.zero;
    }

    public override void Heuristic(float[] actionsOut)
    {
        Array.Clear(actionsOut, 0, actionsOut.Length);

        if (input.GetKey(KeyCode.UpArrow)) actionsOut[HeuristicMovementIndex] += HeuristicMovementUp;
        if (input.GetKey(KeyCode.DownArrow)) actionsOut[HeuristicMovementIndex] += HeuristicMovementDown;
        if (input.GetKey(KeyCode.RightArrow)) actionsOut[HeuristicRotationIndex] += HeuristicRotationRight;
        if (input.GetKey(KeyCode.LeftArrow)) actionsOut[HeuristicRotationIndex] += HeuristicRotationLeft;
    }
}
