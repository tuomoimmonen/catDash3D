using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    [SerializeField] GameObject audioPanel;


    void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("masterVolume", 0));
            mixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 0));
            mixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 0));
            SetSliders();
        }
        else
        {
            SetSliders();
        }
    }

    void Update()
    {
        
    }

    void SetSliders()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 0);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0);
    }

    public void UpdateMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
    }
    public void UpdateSFXVolume()
    {
        mixer.SetFloat("sfxVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
    public void UpdateMusicVolume()
    {
        mixer.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void BackToGame()
    {
        audioPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
