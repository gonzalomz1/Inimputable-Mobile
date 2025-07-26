using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuLoop;
    [SerializeField] private AudioClip menuInteractionSound;
    [SerializeField] private AudioClip MainLoopTheme;
    [SerializeField] private AudioClip pistolShoot;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource menuLoopAudioSource;
    [SerializeField] private AudioSource menuInteractionAudioSource;
    [SerializeField] private AudioSource mainLoopThemeAudioSource;

    [SerializeField] private AudioSource pistolShootAudioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
        SubscribeToEvents();
    }

    private void SetAudioSourceConfiguration()
    {
        if (menuLoopAudioSource != null)
            menuLoopAudioSource.loop = true;
        menuLoopAudioSource.playOnAwake = false;
        if (menuInteractionAudioSource != null)
            menuInteractionAudioSource.loop = false;
        menuInteractionAudioSource.playOnAwake = false;
        if (mainLoopThemeAudioSource != null)
            mainLoopThemeAudioSource.loop = true;
        mainLoopThemeAudioSource.playOnAwake = false;
        if (pistolShootAudioSource != null)
            pistolShootAudioSource.loop = false;
        pistolShootAudioSource.playOnAwake = false;

    }

    private void SubscribeToEvents()
    {
        gameManager.GameExecute += OnGameExecute;
        gameManager.GameplayStart += OnGameplayStartCalled;
        gameManager.AudioPlayMenuInteractionSound += PlayMenuInteractionSound;
        gameManager.AudioStartMenuLoopSong += StartMenuLoopSong;
        gameManager.AudioPistolShoot += PlayPistolShootSound;
    }

    private void OnGameExecute()
    {
        SetAudioSourceConfiguration();
    }


    private void OnGameplayStartCalled()
    {
        StopMenuLoopSong();
        StartCoroutine(DelayedStartMainLoopTheme());
        
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
        menuInteractionAudioSource.enabled = true;
        menuInteractionAudioSource.Play();
    }

    private void StopMenuLoopSong()
    {
        menuInteractionAudioSource.enabled = false;
        menuLoopAudioSource.Stop();
    }

    private void StartMainLoopTheme()
    {
        if (mainLoopThemeAudioSource.clip != MainLoopTheme)
            mainLoopThemeAudioSource.clip = MainLoopTheme;
        mainLoopThemeAudioSource.enabled = true;
        mainLoopThemeAudioSource.Play();
    }

    private void PlayPistolShootSound()
    {
        if (pistolShootAudioSource.clip != pistolShoot)
            pistolShootAudioSource.clip = pistolShoot;
        pistolShootAudioSource.Play();
    }
    
        private IEnumerator DelayedStartMainLoopTheme()
    {
        yield return null; // espera un frame
        StartMainLoopTheme();
    }
}
