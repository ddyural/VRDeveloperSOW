using System.Collections.Generic;
using UnityEngine;
using static Scenario;
using static ScenarioRunner;


// это главна€ точка управлени€ сценари€ми
// этот скрипт сам не выполн€ет шаги
// го задача Ч сказать ScenarioRunner,
// какой сценарий нужно запустить или остановить

// 1. ” нас есть ссылка на ScenarioRunner
// 2. StartScenario(tutorialScenario); -> передаЄт TutorialScenario в ScenarioRunner
// 3. StopScenario(); - стоп сценарию
// 4. ScenarioManager.Instance.IsRunning -> позвол€ет другим системам узнать, идЄт ли сценарий
// 5. ¬ будущем может переключать сценарии
// TutorialScenario завершЄн -> ScenarioManager -> запускает Level1Scenario

// в ScenarioManager Ќ≈“
// следующих проверок:
// вз€л ли игрок предмет
// мишеней
// зоны
// открытием двери
// физикой коробки
// стрельбой
// магнитной пушкой


// ScenarioManager Ч диспетчер сценариев, а ScenarioRunner Ч исполнитель

// менеджер дл€ управлени€ сценари€ми в игре
public class ScenarioManager : MonoBehaviour
{
    // статический экземпл€р дл€ реализации 
    public static ScenarioManager Instance { get; private set; }

    // ссылка на компонент, который запускает сценарии
    [SerializeField]
    private ScenarioRunner runner;

    // инициализаци€ при загрузке объекта
    private void Awake()
    {
        // если экземпл€р уже существует, удал€ем дубликат
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // публичное свойство дл€ проверки активности сценари€
    public bool IsRunning => runner.IsRunning;

    // метод дл€ старта нового сценари€
    public void StartScenario(Scenario scenario)
    {
        // если раннер зан€т, прерываем метод
        if (runner.IsRunning)
            return;


        runner.Run(scenario);  // передаем шаги на выполнение
    }

    // метод дл€ остановки текущего сценари€
    public void StopScenario()
    {
        runner.Stop();
    }
}