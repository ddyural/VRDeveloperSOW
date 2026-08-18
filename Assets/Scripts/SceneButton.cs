using UnityEngine;

// зачем нужен этот скрипт?
// обработка нажати€ и последующее перенаправление игрока на другую сцену
public class SceneButton : MonoBehaviour
{
    // две разных сцены
    // будет выпадающий список - это удобно
    public enum ButtonAction 
    {
        SandBox,
        Training
    }

    [SerializeField]
    private ButtonAction action; // в Inspector могу выбрать сцену

    [SerializeField]
    private SceneLoader sceneLoader; // в Inspector добавл€ю ссылку на объект SceneLoader

    public void Press() // в зависимости от того, что € укажу
    {
        switch (action)
        {
            case ButtonAction.SandBox:
                sceneLoader.LoadScene("SandBox");
                break;

            case ButtonAction.Training:
                sceneLoader.LoadScene("Training");
                break;
        }
    }
}
