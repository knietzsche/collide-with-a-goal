using System.Collections;
using NUnit.Framework;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngineProvider;

namespace UnitTest
{
    public class CustomAgentOnActionReceived
    {
        private CustomAgentMock PrepareActionReceived()
        {
            var trainingAreaObject = new GameObject();
            trainingAreaObject.AddComponent<MeshCollider>();

            var trainingArea = trainingAreaObject.AddComponent<TrainingAreaMock>();
            trainingArea.SetUnityObject(new ObjectMock(Goal.TagDefault));
            trainingArea.SetGoalCount(1);

            var agentObject = new GameObject();
            agentObject.transform.SetParent(trainingAreaObject.transform);
            agentObject.AddComponent<Rigidbody>();

            var behaviorParameters = agentObject.AddComponent<BehaviorParameters>();
            behaviorParameters.BrainParameters.VectorObservationSize = CustomAgent.VectorObservationSize;

            var agent = agentObject.AddComponent<CustomAgentMock>();
            agent.SetTime(new TimeMock(CustomAgent.RotationTimeCoefficient));

            trainingArea.StartRoutine();
            agent.StartRoutine();

            return agent;
        }

        [UnityTest]
        public IEnumerator ActionReceivedMoveForward()
        {
            var agent = PrepareActionReceived();

            var vectorAction = new float[CustomAgent.VectorActionSize];
            vectorAction[CustomAgent.HeuristicMovementIndex] = CustomAgent.HeuristicMovementUp;

            yield return new WaitForEndOfFrame();

            var body = agent.GetComponent<Rigidbody>();
            agent.transform.localPosition = new Vector3(0f, TrainingArea.LocalPositionAgentY, 0f);
            agent.transform.localRotation = Quaternion.identity;
            body.velocity = Vector3.zero;

            agent.OnActionReceived(vectorAction);

            yield return new WaitUntil(() => body.velocity != Vector3.zero);

            Assert.True(body.velocity.z > 0f);
        }

        [UnityTest]
        public IEnumerator ActionReceivedMoveBackward()
        {
            var agent = PrepareActionReceived();

            var vectorAction = new float[CustomAgent.VectorActionSize];
            vectorAction[CustomAgent.HeuristicMovementIndex] = CustomAgent.HeuristicMovementDown;

            yield return new WaitForEndOfFrame();

            var body = agent.GetComponent<Rigidbody>();
            agent.transform.localPosition = new Vector3(0f, TrainingArea.LocalPositionAgentY, 0f);
            agent.transform.localRotation = Quaternion.identity;
            body.velocity = Vector3.zero;

            agent.OnActionReceived(vectorAction);

            yield return new WaitUntil(() => body.velocity != Vector3.zero);

            Assert.True(body.velocity.z < 0f);
        }

        [UnityTest]
        public IEnumerator ActionReceivedRotateRight()
        {
            var agent = PrepareActionReceived();

            var vectorAction = new float[CustomAgent.VectorActionSize];
            vectorAction[CustomAgent.HeuristicRotationIndex] = CustomAgent.HeuristicRotationRight;

            yield return new WaitForEndOfFrame();

            agent.transform.localRotation = Quaternion.identity;
            agent.OnActionReceived(vectorAction);

            yield return new WaitForEndOfFrame();

            Assert.True(agent.transform.localEulerAngles.y < 180f);
        }

        [UnityTest]
        public IEnumerator ActionReceivedRotateLeft()
        {
            var agent = PrepareActionReceived();

            var vectorAction = new float[CustomAgent.VectorActionSize];
            vectorAction[CustomAgent.HeuristicRotationIndex] = CustomAgent.HeuristicRotationLeft;

            yield return new WaitForEndOfFrame();

            agent.transform.localRotation = Quaternion.identity;
            agent.OnActionReceived(vectorAction);

            yield return new WaitForEndOfFrame();

            Assert.True(agent.transform.localEulerAngles.y > 180f);
        }

        [UnityTest]
        public IEnumerator ActionReceivedReward()
        {
            var agent = PrepareActionReceived();

            yield return new WaitForEndOfFrame();

            var vectorAction = new float[CustomAgent.VectorActionSize];
            agent.OnActionReceived(vectorAction);

            Assert.True(agent.GetTotalReward() == TrainingArea.RewardOnActionReceived);
        }
    }
}
