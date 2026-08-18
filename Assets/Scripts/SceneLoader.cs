using UnityEngine;
using UnityEngine.SceneManagement;


// SceneLoader Ч отдельный скрипт, который непосредственно переключает сцену
// LoadScene("TestScene"); -> переключит на TestScene; LoadScene("Training"); -> Training
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
