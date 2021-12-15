using UnityEngine;

public class Goal : MonoBehaviour
{
    public const string TagDefault = "goal";

    private TrainingArea trainingArea;
    
    private void Awake()
    {
        trainingArea = GetComponentInParent<TrainingArea>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("agent"))
        {
            trainingArea.Collect(this);
        }
    }
}
