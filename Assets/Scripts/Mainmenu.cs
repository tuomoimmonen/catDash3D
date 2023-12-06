using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    [SerializeField] GameObject audioPanel;
    public void StartGame()
    {
        //buttonSound.Play();
        SoundManager.instance.PlayAudioClip(8);
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame() //coroutine to get sounds play etc
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleAudioPanel()
    {
        audioPanel.SetActive(!audioPanel.activeInHierarchy);
    }

    public void ResetDataButton()
    {
        SoundManager.instance.PlayAudioClip(8);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey("coins");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
