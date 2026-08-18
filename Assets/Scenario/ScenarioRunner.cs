using System.Collections;
using UnityEngine;


// зачем нужен этот скрипт?
// ScenarioManager — диспетчер сценариев, а ScenarioRunner — исполнитель

// он получает Scenario и последовательно выполняет его шаги, не переходя к следующему, пока текущий не выполнен

/* 1. Запускает сценарий: Scenario -> Runner
 * 2. Берёт шаги по очереди: Step 1 ->  Step 2 ->  Step 3 ->  Step 4 и так до бесконечности
 * пока не надоест
 * 3. Определяет  тип текущего шага: GrabObject, ShootTargets, GrabBox, StandInZone
 * 4. Ждёт, пока игрок реально   выполнит текущий шаг
 * 5.  Проверяет, что выполнена именно нужная задача
 * 6.  Переходит к  следующему шагу
 * 7. Завершает сценарий,   когда все шаги выполнены
 * 8. Может остановить выполнение сценария чрез Stop()
 */


// Класс для последовательного выполнения шагов игрового сценария
public class ScenarioRunner : MonoBehaviour
{
    // Ссылка на текущую запущенную корутину сценария
    private Coroutine runningCoroutine;

    // Проверяет, выполняется ли сценарий в данный момент
    public bool IsRunning { get; private set; }

    public int CurrentStepIndex { get; private set; } = -1;

    public ScenarioStep CurrentStep { get; private set; }

    private bool currentStepCompleted;

    // Запускает переданный сценарий
    public void Run(Scenario scenario)
    {
        // Не запускаем второй сценарий, если текущий ещё выполняется
        if (IsRunning)
            return;

        // проверяем, что сценарий существует
        if (scenario == null)
        {
            Debug.LogError("ScenarioRunner: Сценарий не найден!");
            return;
        }

        // проверяем, что в сценарии есть шаги
        if (scenario.steps == null || scenario.steps.Count == 0)
        {
            Debug.LogWarning("ScenarioRunner: В сценарии, оказывается, нет шагов!");
            return;
        }

        CurrentStepIndex = -1;
        CurrentStep = null;
        currentStepCompleted = false;

        // Запускаем выполнение сценария
        runningCoroutine = StartCoroutine(RunCoroutine(scenario));
    }

    // останавливает текущий сценарий
    public void Stop()
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        IsRunning = false;

        CurrentStepIndex = -1;
        CurrentStep = null;
        currentStepCompleted = false;

        Debug.Log("Сценарий остановлен");
    }

    // Последовательно выполняет все шаги сценария
    private IEnumerator RunCoroutine(Scenario scenario)
    {
        IsRunning = true;

        Debug.Log($"Сценарий начался: {scenario.name}");

        for (int i = 0; i < scenario.steps.Count; i++)
        {
            CurrentStepIndex = i;
            CurrentStep = scenario.steps[i];
            currentStepCompleted = false;

            ScenarioStep step = scenario.steps[i];

            Debug.Log(
                $"Текущий шаг сценария {i + 1}/{scenario.steps.Count}: {step.type}"
            );

            // выполняем текущий шаг и ждём его завершения
            yield return ExecuteStep(step);
        }

        IsRunning = false;
        runningCoroutine = null;

        CurrentStepIndex = -1;
        CurrentStep = null;

        Debug.Log($"Сценарий завершён: {scenario.name}");
    }

    // определяет, какую механику нужно выполнить
    private IEnumerator ExecuteStep(ScenarioStep step)
    {
        switch (step.type)
        {
            case StepType.GrabObject:
                yield return ExecuteGrabObject(step);
                break;

            case StepType.ShootTargets:
                yield return ExecuteShootTargets(step);
                break;

            case StepType.GrabBox:
                yield return ExecuteGrabBox(step);
                break;

            case StepType.PlaceBoxOnPlate:
                yield return ExecutePlaceBoxOnPlate(step);
                break;

            case StepType.StandInZone:
                yield return ExecuteStandInZone(step);
                break;

            case StepType.UseMagneticGun:
                yield return ExecuteMagneticGun(step);
                break;

            default:
                Debug.LogWarning(
                    $"ScenarioRunner: Неизвестный step type: {step.type}"
                );
                break;
        }
    }

    // ---------------------------------------------------------
    // GRAB OBJECT
    // ---------------------------------------------------------

    private IEnumerator ExecuteGrabObject(ScenarioStep step)
    {
        Debug.Log(
            $"Ждём когда же игрок возьмёт объект...: {step.targetId}"
        );

        // ждём заданное время
        // позже здесь будет проверка фактического подбора объекта
        yield return WaitForStepCompletion();

        Debug.Log(
            $"Успешно игрок взял объект: {step.targetId}"
        );
    }

    // ---------------------------------------------------------
    // SHOOT TARGETS
    // ---------------------------------------------------------

    private IEnumerator ExecuteShootTargets(ScenarioStep step)
    {
        Debug.Log(
            $"Ну и когда же игрок выстрелит?: {step.targetId}"
        );

        // ждём заданное время
        // позже здесь будет ожидание уничтожения всех нужных мишеней
        yield return WaitForStepCompletion();

        Debug.Log(
            $"Игрок расстрелял всё: {step.targetId}"
        );
    }

    // ---------------------------------------------------------
    // GRAB BOX
    // ---------------------------------------------------------

    private IEnumerator ExecuteGrabBox(ScenarioStep step)
    {
        Debug.Log(
            $"Игроку надо бы взять коробку: {step.targetId}"
        );

        // ждём заданное время
        yield return WaitForStepCompletion();

        Debug.Log(
            $"Он взял: {step.targetId}"
        );
    }

    // ---------------------------------------------------------
    // PLACE BOX ON PLATE
    // ---------------------------------------------------------

    private IEnumerator ExecutePlaceBoxOnPlate(ScenarioStep step)
    {
        Debug.Log(
            $"Игроку надо положить коробку на нажимную плиту, чтобы активировать её: {step.targetId}"
        );

        // ждём заданное время
        yield return WaitForStepCompletion();

        Debug.Log(
            $"игрок успешно положил коробку на нажимную плиту: {step.targetId}"
        );
    }

    // ---------------------------------------------------------
    // STAND IN ZONE
    // ---------------------------------------------------------

    private IEnumerator ExecuteStandInZone(ScenarioStep step)
    {
        Debug.Log(
            $"Игрок должен войти в зону: {step.targetId}"
        );

        // ждём заданное время
        yield return WaitForStepCompletion();

        Debug.Log(
            $"Он встал в указанную позицию: {step.targetId}"
        );
    }

    // ---------------------------------------------------------
    // MAGNETIC GUN
    // ---------------------------------------------------------

    private IEnumerator ExecuteMagneticGun(ScenarioStep step)
    {
        Debug.Log(
            $"Поиграй с грави пушкой: {step.targetId}"
        );

        // ждём заданное время
        yield return WaitForStepCompletion();

        Debug.Log(
            $"Игрок успешно использовал грави пушку: {step.targetId}"
        );
    }

    private IEnumerator WaitForStepCompletion()
    {
        while (!currentStepCompleted)
        {
            yield return null;
        }
    }

    public bool TryCompleteCurrentStep(string targetId)
    {
        Debug.Log($"=== Попытка завершить шаг ===");
        Debug.Log($"IsRunning: {IsRunning}");
        Debug.Log($"CurrentStep: {CurrentStep}");
        Debug.Log($"CurrentStep.targetId: {CurrentStep?.targetId}");
        Debug.Log($"Полученный targetId: {targetId}");

        if (!IsRunning)
        {
            Debug.LogWarning("Сценарий сейчас НЕ запущен!");
            return false;
        }

        if (CurrentStep == null)
        {
            Debug.LogWarning("CurrentStep == null!");
            return false;
        }

        if (CurrentStep.targetId != targetId)
        {
            Debug.LogWarning(
                $"ID НЕ совпадают! Scenario: '{CurrentStep.targetId}' | Object: '{targetId}'"
            );

            return false;
        }

        currentStepCompleted = true;

        Debug.Log($"ШАГ УСПЕШНО ЗАВЕРШЁН: {targetId}");

        return true;
    }

}
