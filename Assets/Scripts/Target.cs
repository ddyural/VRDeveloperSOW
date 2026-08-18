using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("WaterDrop")]
    public string waterDropTag = "WaterDrop";

    [SerializeField]
    private string targetId;

    // start contact with WaterDrop
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter = true");
        if (other.CompareTag(waterDropTag))
        {
            bool completed = ScenarioManager.Instance.CompleteCurrentStep(targetId);

            if (completed)
            {
                Debug.Log($"Мишень выполнена: {targetId}");
            }

        }
    }
}
