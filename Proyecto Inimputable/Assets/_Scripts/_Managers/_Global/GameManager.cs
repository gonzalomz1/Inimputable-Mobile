using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Managers")]
    public AudioManager audioManager;
    public MenuManager menuManager;
    public GameplayManager gameplayManager;
    public CameraManager cameraManager;

    GlobalState currentGameState;

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
        SubscribeToAudioListeners();
        cameraManager.SwitchToMenuCamera();
        currentGameState = GlobalState.LogoDisplay;
        ManageGlobalState(currentGameState);

        
    }

    private void ManageGlobalState(GlobalState globalState)
    {
        switch (globalState)
        {
            case GlobalState.LogoDisplay:
                menuManager.FromExecuteGameStart();
                break;
            case GlobalState.MenuWithInimputableClip:
                break;
        }
    }

    private void SubscribeToAudioListeners()
    {
        menuManager.InteractionSoundMenu += OnInteractionSoundMenuRequest;
        menuManager.MenuLoopSongRequest += OnMenuLoopSongRequest;
    }



    private void OnInteractionSoundMenuRequest()
    {
        AudioManager.instance.PlayMenuInteractionSound();
    }

    private void OnMenuLoopSongRequest()
    {
        AudioManager.instance.StartMenuLoopSong();
    }
}
