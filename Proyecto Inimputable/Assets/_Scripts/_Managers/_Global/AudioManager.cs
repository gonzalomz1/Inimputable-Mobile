using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuLoop;
    [SerializeField] private AudioClip menuInteractionSound;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource menuLoopAudioSource;
    [SerializeField] private AudioSource menuInteractionAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        SetAudioSourceConfiguration();
    }

    private void SetAudioSourceConfiguration()
    {
        if (menuLoopAudioSource != null)
            menuLoopAudioSource.loop = true;
            menuLoopAudioSource.playOnAwake = false;
        if (menuInteractionAudioSource != null)
            menuInteractionAudioSource.loop = false;
            menuInteractionAudioSource.playOnAwake = false;

    }

    public void StartMenuLoopSong()
    {
        if (menuLoopAudioSource.clip != menuLoop)
            menuLoopAudioSource.clip = menuLoop;
        menuLoopAudioSource.Play();
    }

    public void PlayMenuInteractionSound()
    {
        if (menuInteractionAudioSource.clip != menuInteractionSound)
            menuInteractionAudioSource.clip = menuInteractionSound;
        menuInteractionAudioSource.Play();
    }
}
