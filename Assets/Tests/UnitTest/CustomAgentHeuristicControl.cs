using System.Collections;
using NUnit.Framework;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngineProvider;

namespace UnitTest
{
    public class CustomAgentHeuristicControl
    {
        private const KeyCode KeyCodeUp = KeyCode.UpArrow;
        private const KeyCode KeyCodeDown = KeyCode.DownArrow;
        private const KeyCode KeyCodeRight = KeyCode.RightArrow;
        private const KeyCode KeyCodeLeft = KeyCode.LeftArrow;

        private CustomAgent PrepareHeuristicMovement(IInput input)
        {
            var agentObject = new GameObject();
            agentObject.AddComponent<Rigidbody>();

            var behaviorParameters = agentObject.AddComponent<BehaviorParameters>();
            behaviorParameters.BrainParameters.VectorObservationSize = CustomAgent.VectorObservationSize;

            var agent = agentObject.AddComponent<CustomAgentMock>();
            agent.SetInput(input);

            agent.StartRoutine();

            return agent;
        }

        [UnityTest]
        public IEnumerator HeuristicMovementUp()
        {
            var input = new InputMock(new KeyCode[] { KeyCodeUp });
            var agent = PrepareHeuristicMovement(input);

            var actionsOut = new float[CustomAgent.VectorActionSize];
            agent.Heuristic(actionsOut);

            yield return new WaitForFixedUpdate();

            Assert.True(input.GetCounter() == 1);
            Assert.True(actionsOut[CustomAgent.HeuristicMovementIndex] == CustomAgent.HeuristicMovementUp);
        }

        [UnityTest]
        public IEnumerator HeuristicMovementDown()
        {
            var input = new InputMock(new KeyCode[] { KeyCodeDown });
            var agent = PrepareHeuristicMovement(input);

            var actionsOut = new float[CustomAgent.VectorActionSize];
            agent.Heuristic(actionsOut);

            yield return new WaitForFixedUpdate();

            Assert.True(input.GetCounter() == 1);
            Assert.True(actionsOut[CustomAgent.HeuristicMovementIndex] == CustomAgent.HeuristicMovementDown);
        }

        [UnityTest]
        public IEnumerator HeuristicRotationRight()
        {
            var input = new InputMock(new KeyCode[] { KeyCodeRight });
            var agent = PrepareHeuristicMovement(input);

            agent.transform.localPosition = new Vector3(0f, TrainingArea.LocalPositionAgentY, 0f);
            agent.transform.localRotation = Quaternion.identity;
            var actionsOut = new float[CustomAgent.VectorActionSize];
            agent.Heuristic(actionsOut);

            yield return new WaitForEndOfFrame();

            Assert.True(input.GetCounter() == 1);
            Assert.True(actionsOut[CustomAgent.HeuristicRotationIndex] == CustomAgent.HeuristicRotationRight);
        }

        [UnityTest]
        public IEnumerator HeuristicRotationLeft()
        {
            var input = new InputMock(new KeyCode[] { KeyCodeLeft });
            var agent = PrepareHeuristicMovement(input);

            agent.transform.localPosition = new Vector3(0f, TrainingArea.LocalPositionAgentY, 0f);
            agent.transform.localRotation = Quaternion.identity;
            var actionsOut = new float[CustomAgent.VectorActionSize];
            agent.Heuristic(actionsOut);

            yield return new WaitForEndOfFrame();

            Assert.True(input.GetCounter() == 1);
            Assert.True(actionsOut[CustomAgent.HeuristicRotationIndex] == CustomAgent.HeuristicRotationLeft);
        }
    }
}
