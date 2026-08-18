using UnityEngine;

public class ScenarioStarter : MonoBehaviour
{
    [SerializeField]
    private Scenario scenario;

    private void Start()
    {
        Debug.Log("Запускаем сценарий!!!!!!");

        if (scenario == null)
            Debug.Log("Сценарий не задан....!");

        ScenarioManager.Instance.StartScenario(scenario);
    }
}
