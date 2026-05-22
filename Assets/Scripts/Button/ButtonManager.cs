using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void LoadGameScene(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void LoadGameSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
