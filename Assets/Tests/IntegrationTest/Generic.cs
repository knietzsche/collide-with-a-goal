using NUnit.Framework;
using UnityEngineProvider;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.MLAgents.Policies;

namespace IntegrationTest
{
    public class Generic
    {
        private TrainingAreaMock PrepareInitializationGoalCount(int goalCount)
        {
            var trainingAreaObject = new GameObject();
            trainingAreaObject.AddComponent<MeshCollider>();
            
            var trainingArea = trainingAreaObject.AddComponent<TrainingAreaMock>();
            trainingArea.SetUnityObject(new ObjectMock(Goal.TagDefault));
            trainingArea.SetGoalCount(goalCount);

            var agentObject = new GameObject();
            agentObject.transform.SetParent(trainingAreaObject.transform);
            agentObject.transform.localPosition = new Vector3(0f, TrainingArea.LocalPositionAgentY, 0f);
            agentObject.AddComponent<Rigidbody>();

            var behaviorParameters = agentObject.AddComponent<BehaviorParameters>();
            behaviorParameters.BrainParameters.VectorObservationSize = CustomAgent.VectorObservationSize;

            var agent = agentObject.AddComponent<CustomAgentMock>();
          
            trainingArea.StartRoutine();
            agent.StartRoutine();

            return trainingArea;
        }

        [UnityTest]
        public IEnumerator InitializationAgentPlacementFixed()
        {
            var trainingArea = PrepareInitializationGoalCount(1);
            trainingArea.SetRandomAgentPlacement(false);
            var agent = trainingArea.GetComponentInChildren<CustomAgent>();

            trainingArea.ResetEpisode();
            Assert.True(agent.transform.localPosition.x == 0f);
            Assert.True(agent.transform.localPosition.z == 0f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator InitializationAgentPlacementRandom()
        {
            var trainingArea = PrepareInitializationGoalCount(1);
            trainingArea.SetRandomAgentPlacement(true);
            trainingArea.SetRandomSpawnRange(new RandomMock<float>(new float[] { 3f, 5f, 2f, 8f }));
            var agent = trainingArea.GetComponentInChildren<CustomAgent>();
            
            trainingArea.ResetEpisode();
            Assert.True(agent.transform.localPosition.x == 3f);
            Assert.True(agent.transform.localPosition.z == 5f);

            trainingArea.ResetEpisode();
            Assert.True(agent.transform.localPosition.x == 2f);
            Assert.True(agent.transform.localPosition.z == 8f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator InitializationGoalCount_1()
        {
            var trainingArea = PrepareInitializationGoalCount(1);

            yield return new WaitForEndOfFrame();

            Assert.True(trainingArea.GetComponentsInChildren<Goal>().Length == 1);
        }

        [UnityTest]
        public IEnumerator InitializationGoalCount_3()
        {
            var trainingArea = PrepareInitializationGoalCount(5);

            yield return new WaitForEndOfFrame();

            Assert.True(trainingArea.GetComponentsInChildren<Goal>().Length == 5);
        }

        [UnityTest]
        public IEnumerator ResetEpisodeSpawningGoalCount_5()
        {
            var trainingArea = PrepareInitializationGoalCount(5);

            yield return new WaitForEndOfFrame();

            var goalsBefore = trainingArea.GetComponentsInChildren<Goal>();
            Assert.True(goalsBefore.Length == 5);

            trainingArea.GetComponent<TrainingArea>().ResetEpisode();

            yield return new WaitForEndOfFrame();

            var goalsAfter = trainingArea.GetComponentsInChildren<Goal>();
            Assert.True(goalsAfter.Length == 5);

            foreach (var goalBefore in goalsBefore)
            {
                foreach (var goalAfter in goalsAfter)
                {
                    Assert.True(goalBefore != goalAfter);
                }
            }
        }

        [UnityTest]
        public IEnumerator CollectGoalEpisodeEnd()
        {
            var trainingArea = PrepareInitializationGoalCount(1);

            yield return new WaitForEndOfFrame();

            var agent = trainingArea.GetComponentInChildren<CustomAgentMock>();
            Assert.True(agent.CompletedEpisodes == 0);

            trainingArea.Collect(trainingArea.GetComponentInChildren<Goal>());

            Assert.True(agent.CompletedEpisodes == 1);
        }

        [UnityTest]
        public IEnumerator CollectGoalEpisodeNotEnd()
        {
            var trainingArea = PrepareInitializationGoalCount(2);

            yield return new WaitForEndOfFrame();

            trainingArea.Collect(trainingArea.GetComponentInChildren<Goal>());

            var agent = trainingArea.GetComponentInChildren<CustomAgentMock>();
            Assert.True(agent.CompletedEpisodes == 0);
        }

        [UnityTest]
        public IEnumerator CollectGoalEpisodeNotEndReward()
        {
            var trainingArea = PrepareInitializationGoalCount(2);

            yield return new WaitForEndOfFrame();

            var agent = trainingArea.GetComponentInChildren<CustomAgentMock>();
            Assert.True(agent.GetTotalReward() == 0f);

            trainingArea.Collect(trainingArea.GetComponentInChildren<Goal>());

            Assert.True(agent.GetTotalReward() == TrainingArea.RewardGoalCollected);
        }

        [UnityTest]
        public IEnumerator CollectGoalEpisodeEndReward()
        {
            var trainingArea = PrepareInitializationGoalCount(1);

            yield return new WaitForEndOfFrame();

            var agent = trainingArea.GetComponentInChildren<CustomAgentMock>();
            Assert.True(agent.GetTotalReward() == 0f);

            trainingArea.Collect(trainingArea.GetComponentInChildren<Goal>());

            Assert.True(agent.GetTotalReward() == TrainingArea.RewardGoalCollected + TrainingArea.RewardEndEpisode);
        }

        private TrainingAreaMock PrepareIntializationGoalCountRandom(int[] goalCounts)
        {
            var trainingAreaObject = new GameObject();
            
            var trainingArea = trainingAreaObject.AddComponent<TrainingAreaMock>();
            trainingArea.SetUnityObject(new ObjectMock(Goal.TagDefault));
            trainingArea.SetGoalCount(0);
            trainingArea.SetRandomGoalCount(new RandomMock<int>(goalCounts));

            var agentObject = new GameObject();
            agentObject.transform.SetParent(trainingAreaObject.transform);
            agentObject.transform.localPosition = new Vector3(0f, TrainingArea.LocalPositionAgentY, 0f);
            agentObject.AddComponent<Rigidbody>();

            var behaviorParameters = agentObject.AddComponent<BehaviorParameters>();
            behaviorParameters.BrainParameters.VectorObservationSize = CustomAgent.VectorObservationSize;

            agentObject.AddComponent<CustomAgentMock>();

            return trainingArea;
        }

        [UnityTest]
        public IEnumerator InitializationGoalCountRandom_1()
        {
            var trainingArea = PrepareIntializationGoalCountRandom(new int[] { 1 });
            trainingArea.StartRoutine();
            trainingArea.GetComponentInChildren<CustomAgentMock>().StartRoutine();

            yield return new WaitForEndOfFrame();

            var goals = trainingArea.GetComponentsInChildren<Goal>();
            Assert.True(goals.Length == 1);
        }

        [UnityTest]
        public IEnumerator InitializationGoalCountRandom_3_5()
        {
            var trainingArea = PrepareIntializationGoalCountRandom(new int[] { 3, 5 });
            trainingArea.StartRoutine();
            trainingArea.GetComponentInChildren<CustomAgentMock>().StartRoutine();

            yield return new WaitForEndOfFrame();

            Assert.True(trainingArea.GetComponentsInChildren<Goal>().Length == 3);

            trainingArea.GetComponent<TrainingArea>().ResetEpisode();

            yield return new WaitForEndOfFrame();

            Assert.True(trainingArea.GetComponentsInChildren<Goal>().Length == 5);
        }

        [UnityTest]
        public IEnumerator InitializationGoalCount_Max_NoOverlap()
        {
            var trainingArea = PrepareInitializationGoalCount(int.MaxValue);

            yield return new WaitForEndOfFrame();

            var goals = trainingArea.GetComponentsInChildren<Goal>();
            var agent = trainingArea.GetComponentInChildren<CustomAgent>();

            foreach (var goal in goals)
            {
                foreach (var goalOther in goals)
                {
                    if (goalOther != goal) Assert.True(Vector3.Distance(goal.transform.localPosition, goalOther.transform.position) > TrainingArea.DistanceMinGoalGoal);
                }
                Assert.True(Vector3.Distance(goal.transform.localPosition, agent.transform.localPosition) > TrainingArea.DistanceMinAgentGoal);
            }
        }
    }
}
