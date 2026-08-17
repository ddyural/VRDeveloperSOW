using System.Collections.Generic;
using UnityEngine;

// для создания сценариев через меню unity
[CreateAssetMenu(menuName = "Scenario/Scenario")]
public class Scenario : ScriptableObject
{
    // список шагов, составляющих весь сценарий
    public List<ScenarioStep> steps;
}

// зачем нужен этот скрипт?
// чтобы Я через список(list) указал каждый шаг ну и последовательность как бонус

// этот кусок кода хранит весь сценарий: список его шагов; это и есть ScriptableObject