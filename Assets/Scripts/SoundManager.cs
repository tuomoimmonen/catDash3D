using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource[] audioSources;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void PlayAudioClip(int audioClipIndex)
    {
        float randomPitch = Random.Range(0.5f, 1.5f);
        audioSources[audioClipIndex].pitch = randomPitch;
        audioSources[audioClipIndex].Play();
    }

    public void StopAudioClips()
    {
        for(int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
    }
}
