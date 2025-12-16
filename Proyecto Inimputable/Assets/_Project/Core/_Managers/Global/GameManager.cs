using System;
using System.Collections;
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
    

    public PlayerPresenter playerPresenter;
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
    public event Action GameRetry;
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

    public bool IsGamePaused => currentGameState == GlobalState.PausedGameplay || currentGameState == GlobalState.MainMenu || currentGameState == GlobalState.GameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        SubscribeToAllEvents();
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
                GameplayResume?.Invoke();
                break;
            case GlobalState.PausedGameplay:
                FreezeTime();
                break;
            case GlobalState.GameOver:
                FreezeTime();
                EnableInput(); // CRITICAL: Input must be enabled for UI interaction!
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

    private void SubscribeToAllEvents()
    {
        SubscribeToAudioListeners();
        SubscribeToCallbacks();
        SubscribeToPlayerEvents();
        SubscribeToCanvasManagerEvents();
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
        currentGameState = GlobalState.Gameplay; // Explicitly set to Gameplay
        ManageGlobalState(currentGameState);     // Ensure Time flows and Input is ready
        FadeOutScreen();

        // TELEPORT PLAYER TO START / CURRENT OBJECTIVE
        if (ObjectiveManager.Instance != null && playerPresenter != null)
        {
             Transform spawnPoint = ObjectiveManager.Instance.GetRespawnPoint();
             Vector3 respawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
             Quaternion respawnRot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

             var playerData = playerPresenter.GetComponent<PlayerData>();
             if (playerData != null)
             {
                 // Use Revive to reset health/stats too, which is appropriate for a fresh start
                 playerData.Revive(respawnPos, respawnRot);
             }
        }
        
        // RESET ETERNAL WORLD STATE (Doors, Interactables)
        ResetWorld();
    }

    private void ResetWorld()
    {
        // Find all doors and reset them to Default state
        Door[] doors = FindObjectsOfType<Door>();
        foreach (var door in doors)
        {
            if (door != null) door.DefaultStateDoor();
        }
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

    private void OnPlayerDead()
    {
        currentGameState = GlobalState.GameOver;
        ManageGlobalState(currentGameState);

        // Show Game Over UI via GameplayMenuCanvas
        if (GameplayMenuCanvas.instance != null)
        {
            GameplayMenuCanvas.instance.SetLoseState();
            GameplayMenuCanvas.instance.gameObject.SetActive(true); // Ensure canvas is active
        }
    }

    public void RetryGame()
    {
        // 1. Reset Global Logic
        // Explicitly set state to Gameplay to unfreeze time and unlock Gameplay inputs
        currentGameState = GlobalState.Gameplay;
        ManageGlobalState(currentGameState);
        
        EnableInput();
        if (canvasManager != null) canvasManager.ClearFingerRoles(); // Prevent stale inputs
        // UI Hiding is handled by the button click in GameplayMenuCanvas or here if preferred
        if (GameplayMenuCanvas.instance != null) 
            GameplayMenuCanvas.instance.gameObject.SetActive(false);

        // 2. Reset Weapons (Visual & Stats)
        if (WeaponController.instance != null) WeaponController.instance.ResetWeapons();

        // 3. Reset Objectives (Timer & Difficulty)
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ResetCurrentObjective();
            
            // 4. Get Spawn Point
            Transform spawnPoint = ObjectiveManager.Instance.GetRespawnPoint();
            Vector3 respawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            Quaternion respawnRot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

            // 5. Revive Player
            if (playerPresenter != null) 
            {
                 var playerData = playerPresenter.GetComponent<PlayerData>();
                 if (playerData != null)
                 {
                     playerData.Revive(respawnPos, respawnRot);
                 }
            }
        }

        // 6. Notify Retry Listeners (Ammo/Health)
        GameRetry?.Invoke();
    }

    public void QuitToMenu()
    {
        ResumeTime();
        // Prevent stale inputs during transition
        if (canvasManager != null) canvasManager.ClearFingerRoles(); 

        // 1. Reset Game Logic (Objectives, etc)
        GameplayExit?.Invoke(); // Notifies ObjectiveManager to reset index
        if (SurvivalDifficultyManager.Instance != null) SurvivalDifficultyManager.Instance.ResetDifficulty();
        if (Spawner.instance != null) Spawner.instance.ClearAllActiveEnemies();
        if (WeaponController.instance != null) WeaponController.instance.ResetWeapons();

        // 2. Hide Gameplay UI
        if (GameplayMenuCanvas.instance != null) GameplayMenuCanvas.instance.gameObject.SetActive(false);
        if (UICanvas.Instance != null) UICanvas.Instance.SetActiveCanvas(false);
        if (PlayerActionsCanvas.Instance != null) PlayerActionsCanvas.Instance.gameObject.SetActive(false);
        
        // Ensure CanvasManager resets to Menu context
        if (CanvasManager.Instance != null) CanvasManager.Instance.ResetToMainMenu();

        // 3. Switch Camera
        if (cameraManager != null) cameraManager.SwitchToMenuCamera();

        // 4. Trigger Menu Flow (Logo -> Menu)
        if (MenuManager.instance != null)
        {
            MenuManager.instance.ReturnToMainMenuDirectly();
        }
    }

    private void FreezeTime()
    {
        Time.timeScale = 0f;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }


    public void HideAllCanvas()
    {
        HideGameplayDefaultCanvas?.Invoke();
    }
    public void ShowAllCanvas()
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

    public void SetWinGame()
    {
        currentGameState = GlobalState.GameOver; // Pauses game and enables input
        ManageGlobalState(currentGameState);

        if (GameplayMenuCanvas.instance != null)
        {
            GameplayMenuCanvas.instance.SetWinState();
            GameplayMenuCanvas.instance.gameObject.SetActive(true);
        }
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
