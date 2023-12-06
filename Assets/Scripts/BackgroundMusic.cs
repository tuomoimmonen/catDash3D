using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip[] gameMusic;

    AudioSource musicSource;
    int musicIndex = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        musicSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        //musicSource.volume = 1.0f;
        musicSource.Play();
    }
    private void Update()
    {
        if(!musicSource.isPlaying && SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayNextSong();
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //scene gets name, buildindex
    //loadscenemode = single or additive ei vaikuta
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip targetClip = null;

        if (scene.buildIndex == 0)
        {
            targetClip = mainMenuMusic;
            //musicSource.volume = 0.1f;
            musicSource.loop = true;
        }
        else if (scene.buildIndex != 0)
        {
            targetClip = gameMusic[musicIndex];
            //musicSource.volume = 0.05f;
            musicSource.loop = false;
        }

        if (musicSource.clip != targetClip)
        {
            musicSource.clip = targetClip;
            musicSource.Play();
        }
    }

    void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % gameMusic.Length;
        musicSource.clip = gameMusic[musicIndex];
        musicSource.Play();
    }
}
