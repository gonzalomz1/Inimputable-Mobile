using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private InputManager inputManager;

    [Header("UI")]
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private ScreenFader screenFader;

    [Header("Player")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private PlayerPresenter playerPresenter;
    [SerializeField] private UIPlayerStats uiPlayerStats;

    [Header("Level Loader")]
    [SerializeField] private LevelLoader levelLoader;

    [SerializeField] private GlobalState currentGameState;

    public event Action GameExecute;
    public event Action MainMenu;
    public event Action GameplayStart;
    public event Action GameplayPause;
    public event Action GameplayResume;
    public event Action GameplayExit;
    public event Action GameplayShowControls;

    public event Action ShowGameplayDefaultCanvas;
    public event Action HideGameplayDefaultCanvas;

    public event Action AudioPlayMenuInteractionSound;
    public event Action AudioStartMenuLoopSong;
    public event Action AudioPistolShoot;
    public event Action AudioStepSound;
    public event Action AudioDoorTransition;

    public event Action FadeIn;
    public event Action FadeOut;

    public event Action GameOver;

    public bool IsGamePaused => currentGameState == GlobalState.PausedGameplay || currentGameState == GlobalState.MainMenu;

    private bool gameplay_loaded = false;

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
        SubscribeToCallbacks();
        SubscribeToPlayerEvents();
        SubscribeToCanvasManagerEvents();
        GameStartMode();
        cameraManager.SwitchToMenuCamera();
        currentGameState = GlobalState.FromExecute;
        ManageGlobalState(currentGameState);
    }

    private void ManageGlobalState(GlobalState globalState)
    {
        switch (globalState)
        {
            case GlobalState.FromExecute:
                DisableInput();
                FromExecuteGameStart();
                break;
            case GlobalState.Loading:
                StartGameplay();
                FadeOutScreen();
                EnableInput();
                break;
            case GlobalState.MainMenu:
                break;
            case GlobalState.Gameplay:
                ResumeTime();
                break;
            case GlobalState.PausedGameplay:
                FreezeTime();
                break;
        }
    }

    private void FromExecuteGameStart()
    {
        StartCoroutine(DelayedGameExecute());
    }

    private IEnumerator DelayedGameExecute()
    {
        yield return new WaitForSeconds(0.1f);
        GameExecute?.Invoke();
    }

    public void StartGameplay()
    {
        GameplayStart.Invoke();
    }

    public void PauseGameplay()
    {
        GameplayPause.Invoke();
    }

    public void ResumeGameplay()
    {
        GameplayResume.Invoke();
    }

    private void GameStartMode()
    {
        DisableInput();
    }
    public void EnableInput()
    {
        inputManager.gameObject.SetActive(true);
    }

    public void DisableInput()
    {
        inputManager.gameObject.SetActive(false);
    }

    private void SubscribeToAudioListeners()
    {
        menuManager.InteractionSoundMenu += OnInteractionSoundMenuRequest;
        menuManager.MenuLoopSongRequest += OnMenuLoopSongRequest;
    }

    private void SubscribeToCallbacks()
    {
        menuManager.ActivateInputs += OnMenuManagerActivateInputs;
        menuManager.StartGameplay += OnMenuManagerStartGameplay;
        gameplayManager.GameplayLoaded += OnGameplayLoaded;
    }

    private void SubscribeToPlayerEvents()
    {
        playerPresenter.Alive += OnPlayerAlive;
        playerPresenter.Dead += OnPlayerDead;
        playerPresenter.TakeDamage += OnPlayerTakeDamage;
    }

    private void SubscribeToCanvasManagerEvents()
    {
        canvasManager.PauseGameplay += OnPauseGameplayCalled;
        canvasManager.ResumeGameplay += OnResumeGameplayCalled;
    }

    public void FadeInScreen()
    {
        FadeIn?.Invoke();
    }

    public void FadeOutScreen()
    {
        FadeOut?.Invoke();
    }

    private void OnPauseGameplayCalled()
    {
        currentGameState = GlobalState.PausedGameplay;
        ManageGlobalState(currentGameState);
    }

    private void OnResumeGameplayCalled()
    {
        currentGameState = GlobalState.Gameplay;
        ManageGlobalState(currentGameState);
    }

    private void OnInteractionSoundMenuRequest()
    {
        AudioPlayMenuInteractionSound.Invoke();
    }

    private void OnMenuLoopSongRequest()
    {
        AudioStartMenuLoopSong.Invoke();
    }

    private void OnGameplayLoaded()
    {
        gameplay_loaded = true;
        FadeOutScreen();
    }

    private void OnMenuManagerActivateInputs()
    {
        EnableInput();
    }

    private void OnMenuManagerStartGameplay()
    {
        currentGameState = GlobalState.Loading;
        ManageGlobalState(currentGameState);
    }

    private void OnPlayerAlive(){ }

    private void OnPlayerDead(){ }

    private void OnPlayerTakeDamage(){ }

    public void PlayerDead(){ }

    private void FreezeTime()
    {
        Time.timeScale = 0f;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    public IEnumerator DoDoorTransition(Door linkedDoor)
    {
        FadeInScreen();
        DisableInput();
        HideAllCanvas();
        GameManager.instance.TriggerDoorTransitionSound();
        playerCC.enabled = false;
        playerRoot.transform.SetPositionAndRotation(
            linkedDoor.GetPlayerSpawnPosition(),
            linkedDoor.GetSpawnRotation()
        );
        Debug.Log($"{linkedDoor.GetPlayerSpawnPosition()}, {linkedDoor.GetSpawnRotation()}");
        playerCC.enabled = true;
        yield return new WaitForSeconds(0.5f);
        EnableInput();
        FadeOutScreen();
        ShowAllCanvas();
    }

    private void HideAllCanvas()
    {
        HideGameplayDefaultCanvas?.Invoke();
    }
    private void ShowAllCanvas()
    {
        ShowGameplayDefaultCanvas?.Invoke();
    }

    public void PlayPistolShootSound()
    {
        AudioPistolShoot?.Invoke();
    }

    public void SetGameOver()
    {
        GameOver?.Invoke();
    }

    public void PlayerMovementSound()
    {
        AudioStepSound?.Invoke();
    }

    public void TriggerDoorTransitionSound()
    {
        AudioDoorTransition?.Invoke();
    }
}
