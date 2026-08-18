using System.Collections.Generic;
using UnityEngine;
using static Scenario;
using static ScenarioRunner;


// это главная точка управления сценариями
// этот скрипт сам не выполняет шаги
// го задача — сказать ScenarioRunner,
// какой сценарий нужно запустить или остановить

// 1. У нас есть ссылка на ScenarioRunner
// 2. StartScenario(tutorialScenario); -> передаёт TutorialScenario в ScenarioRunner
// 3. StopScenario(); - стоп сценарию
// 4. ScenarioManager.Instance.IsRunning -> позволяет другим системам узнать, идёт ли сценарий
// 5. Завершить текущий шаг
// 6. В будущем может переключать сценарии
// TutorialScenario завершён -> ScenarioManager -> запускает Level1Scenario

// в ScenarioManager НЕТ
// следующих проверок:
// взял ли игрок предмет
// мишеней
// зоны
// открытием двери
// физикой коробки
// стрельбой
// магнитной пушкой


// ScenarioManager — диспетчер сценариев, а ScenarioRunner — исполнитель

// менеджер для управления сценариями в игре
public class ScenarioManager : MonoBehaviour
{
    // статический экземпляр для реализации 
    public static ScenarioManager Instance { get; private set; }

    // ссылка на компонент, который запускает сценарии
    [SerializeField]
    private ScenarioRunner runner;

    // инициализация при загрузке объекта
    private void Awake()
    {
        // если экземпляр уже существует, удаляем дубликат
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // публичное свойство для проверки активности сценария
    public bool IsRunning => runner.IsRunning;

    // метод для старта нового сценария
    public void StartScenario(Scenario scenario)
    {
        // если раннер занят, прерываем метод
        if (runner.IsRunning)
            return;


        runner.Run(scenario);  // передаем шаги на выполнение
    }

    // метод для остановки текущего сценария
    public void StopScenario()
    {
        runner.Stop();
    }

    // завершить текущий шаг
    public bool CompleteCurrentStep(string targetId)
    {
        return runner.TryCompleteCurrentStep(targetId);
    }

}