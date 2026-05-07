
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
        // Replace "MainGame" with your actual main scene name.
        SceneManager.LoadScene("MainGame");
    }
    public void TestGame()
    {
        SceneManager.LoadScene("text");
    }

    public void ExitGame()
    {
        // Quit the game.
        Application.Quit();
        // In the Unity Editor, this call does not close play mode; it works in a built game.
        Debug.Log("Game is exiting.");
    }
}
