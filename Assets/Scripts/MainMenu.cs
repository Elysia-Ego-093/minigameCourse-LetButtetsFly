
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button testButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(ExitGame);
        testButton.onClick.AddListener(TestGame);
    }

    public void StartGame()
    {
        // "MainGame" 应替换为你游戏主场景的名字
        SceneManager.LoadScene("MainGame");
    }
    public void TestGame()
    {
        SceneManager.LoadScene("text");
    }

    public void ExitGame()
    {
        // 退出游戏
        Application.Quit();
        // 在Unity编辑器中，这行代码不会生效，但会在最终构建的游戏中生效
        Debug.Log("游戏已退出！");
    }
}
