using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public AudioClip sound;
    public void LoadGameScene(string gameSceneName)
    {
        PlaySound();
        SceneManager.LoadScene(gameSceneName);
    }
    public void LoadGameSceneByIndex(int index)
    {
        PlaySound();
        SceneManager.LoadScene(index);
    }
    public void QuitGame()
    {
        PlaySound();
        Application.Quit();
    }
    public void ClearPlayerData()
    {
        GameData.Instance.ClearPlayerData();
    }
    public void PlaySound()
    {
        if (sound != null)
        {
            GameObject newSoundObject = new GameObject();
            ButtonSound newAudioSource = newSoundObject.AddComponent<ButtonSound>();
            newAudioSource.sound = newSoundObject.AddComponent<AudioSource>();
            newAudioSource.sound.clip = sound;
            newAudioSource.sound.volume = GameData.Instance.SoundVolume;
            newAudioSource.sound.Play();
            DontDestroyOnLoad(newSoundObject);
        }
    }
}
