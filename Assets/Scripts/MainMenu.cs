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
        // 使用存在的Battleground_1场景
        SceneManager.LoadScene("Battleground_1");
    }
    public void TestGame()
    {
        SceneManager.LoadScene("text");
    }

    public void ExitGame()
    {
        // 退出游戏
        Application.Quit();
        // 在Unity编辑器中，此代码不会生效，仅在构建后的游戏中有效
        Debug.Log("游戏已退出");
    }
}