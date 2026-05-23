using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : ButtonManager
{
    [Header("ÔÝÍ£²Ëµ¥")]
    public GameObject PauseMenu;

    [Header("Íæ¼Ò")]
    public List<PlayerController> players;

    private bool isPause = false;
    void Start()
    {
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        foreach (var player in players)
        {
            player.isPause = isPause;
        }
    }

    private void TogglePause()
    {
        if (isPause) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(isPause);
    }

    public void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(isPause);
    }

    public void RestartGame()
    {
        ResumeGame();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public new void LoadGameScene(string gameSceneName)
    {
        ResumeGame();
        SceneManager.LoadScene(gameSceneName);
    }
    public new void LoadGameSceneByIndex(int index)
    {
        ResumeGame();
        SceneManager.LoadScene(index);
    }
}
