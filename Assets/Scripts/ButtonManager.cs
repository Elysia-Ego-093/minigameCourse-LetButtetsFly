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
    public void SelectPlayer_release()
    {
        for(int i = GameData.Instance.PlayerCount - 1; i >= 0; i--)
        {
            if (GameData.Instance.players[i].maxHp != 0)
            {
                GameData.Instance.players[i] = null;
                return;
            }
        }
    }
    public void SelectPlayer_continue()
    {
        foreach(var player in GameData.Instance.players)
        {
            if (player.maxHp == 0) return;
        }
        SceneManager.LoadScene("SelectMap");
    }
}
