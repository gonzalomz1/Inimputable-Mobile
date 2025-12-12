using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    [Header("Input Manager")]
    public InputManager inputManager;
    [Header("Menu")]
    public MenuManager menuManager;
    public GameObject mainMenuCanvas;
    public GameObject mainMenuButtons;
    public GameObject mainMenuControls;
    [Header("Gameplay Canvases")]
    public GameplayMenuCanvas gameplayMenuCanvas;
    public PlayerActionsCanvas PlayerActionsCanvas;
    public MovAndAimCanvas movAndAimCanvas;
    public UICanvas uICanvas;

    [SerializeField] private Dictionary<int, FingerRole> fingerRoles = new Dictionary<int, FingerRole>();

    public event Action PauseGameplay;  
    public event Action ResumeGameplay;

    public void ClearFingerRoles()
    {
        fingerRoles.Clear();
    }

    public event Action RequestShowControlsToGameplayMenuCanvas;
    public event Action RequestHideControlsToGameplayMenuCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SubscribeToGameManagerEvents();
        SubscribeToMenuManagerEvents();
        SubscribeToGameplayMenuCanvasEvents();
        SubscribeToActionCanvasEvents();
    }

    void SubscribeToGameManagerEvents()
    {
        GameManager.instance.GameExecute += OnGameExecute;
        GameManager.instance.GameplayStart += OnGameplayStart;
        GameManager.instance.GameplayPause += OnGameplayPause;
        GameManager.instance.ShowGameplayDefaultCanvas += OnShowGameplayDefaultCanvas;
        GameManager.instance.HideGameplayDefaultCanvas += OnHideGameplayDefaultCanvas;
    }

    void SubscribeToMenuManagerEvents()
    {
        menuManager.CanvasMenuDisabled += OnCanvasMenuDisabled;
        menuManager.InGameplayControls += OnInGameplayControls;
    }

    void SubscribeToGameplayMenuCanvasEvents()
    {
        gameplayMenuCanvas.ResumeGameplayRequest += OnResumeGameplayRequest;
        gameplayMenuCanvas.ShowControlsRequest += OnShowControlsRequest;
    }

    void SubscribeToActionCanvasEvents()
    {
        PlayerActionsCanvas.pauseRequest += OnActionCanvasPauseRequest;
    }
    void OnGameExecute()
    {
        DisableAllExceptMainMenu();
    }

    void OnGameplayStart()
    {
        GameplayMode();
    }

    void OnGameplayPause()
    {
        PauseMode();
    }

    void ShowGameplayMenu()
    {
        gameplayMenuCanvas.gameObject.SetActive(true);
    }

    void OnCanvasMenuDisabled()
    {
        DisableMainMenuStuff();
        DisableAllExceptMainMenu();
    }

    void OnInGameplayControls()
    {
        ShowControlsInGameplay();
    }

    void OnResumeGameplayRequest()
    {
        HideGameplayMenu();
        ResumeGameplay?.Invoke();
    }

    void OnShowControlsRequest()
    {
        RequestShowControlsToGameplayMenuCanvas?.Invoke();    
    }

    void OnShowGameplayDefaultCanvas()
    {
        gameplayMenuCanvas.gameObject.SetActive(false);
        PlayerActionsCanvas.gameObject.SetActive(true);
        uICanvas.gameObject.SetActive(true);
        movAndAimCanvas.gameObject.SetActive(true);
    }

    void OnHideGameplayDefaultCanvas()
    {
        gameplayMenuCanvas.gameObject.SetActive(false);
        PlayerActionsCanvas.gameObject.SetActive(false);
        uICanvas.gameObject.SetActive(false);
        movAndAimCanvas.gameObject.SetActive(false);
    }


    void OnActionCanvasPauseRequest()
    {
        PauseMode();
        PauseGameplay?.Invoke();
    }

    private void GameplayMode()
    {
        DisableAll();
        ShowOnlyGameplayStuff();
    }

    private void DisableAll()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        gameplayMenuCanvas.gameObject.SetActive(false);
        PlayerActionsCanvas.gameObject.SetActive(false);
        movAndAimCanvas.gameObject.SetActive(false);
        uICanvas.gameObject.SetActive(false);
    }

    private void DisableAllExceptMainMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        // Ensure Main Menu Buttons are visible (might have been hidden by Controls/Options)
        if (mainMenuButtons != null) mainMenuButtons.gameObject.SetActive(true);
        
        gameplayMenuCanvas.gameObject.SetActive(false);
        PlayerActionsCanvas.gameObject.SetActive(false);
        movAndAimCanvas.gameObject.SetActive(false);
        uICanvas.gameObject.SetActive(false);
    }

    public void ResetToMainMenu()
    {
        DisableAllExceptMainMenu();
    }

    private void DisableMainMenuStuff()
    {
        mainMenuCanvas.gameObject.SetActive(false);   
    }

    private void HideGameplayMenu()
    {
        gameplayMenuCanvas.gameObject.SetActive(false);
    }

    void ShowOnlyGameplayStuff()
    {
        PlayerActionsCanvas.gameObject.SetActive(true);
        movAndAimCanvas.gameObject.SetActive(true);
        uICanvas.gameObject.SetActive(true);
    }

    private void ShowControlsInGameplay()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        mainMenuButtons.gameObject.SetActive(false);
        mainMenuControls.gameObject.SetActive(true);
    }

    public void SetMenuCanvas(bool boolean)
    {
        gameplayMenuCanvas.gameObject.SetActive(boolean);
    }

    public void PauseMode()
    {
        gameplayMenuCanvas.SetActivePause();
    }

    public void GameOver()
    {
        gameplayMenuCanvas.GameOver();
    }

    public void OnEnable()
    {
        inputManager.OnFingerDown += HandleFingerDown;
        inputManager.OnFingerMove += HandleFingerMove;
        inputManager.OnFingerUp += HandleFingerUp;
    }

    public void OnDisable()
    {
        if (inputManager == null) return;
        inputManager.OnFingerDown -= HandleFingerDown;
        inputManager.OnFingerMove -= HandleFingerMove;
        inputManager.OnFingerUp -= HandleFingerUp;
    }

    void HandleFingerDown(Finger finger)
    {
        // Don't let have multiple fingers on an index
        if (fingerRoles.ContainsKey(finger.index))
        {
            //Debug.LogWarning($"Finger {finger.index} ya está asignado a un rol ({fingerRoles[finger.index]}), ignorando nueva asignación.");
            return;
        }
        if (menuManager.HandleTouch(finger, out FingerRole role))
        {
            fingerRoles[finger.index] = role;
            return;
        }
        if (gameplayMenuCanvas.HandleTouch(finger, out role))
        {
            fingerRoles[finger.index] = role;
            //Debug.Log($"Finger asignado a {role}.");
            return;
        }
        if (GameManager.instance.IsGamePaused) return;
        if (PlayerActionsCanvas.HandleTouch(finger, out role))
        {
            fingerRoles[finger.index] = role;
            //Debug.Log($"Finger asignado a {role}.");
            return;
        }
        if (movAndAimCanvas.HandleTouch(finger, out role))
        {
            fingerRoles[finger.index] = role;
            //Debug.Log($"Finger {finger.index} asignado a {role}");
            return;
        }
        // If not action neither move and aim
        fingerRoles[finger.index] = FingerRole.None;
    }

    private void HandleFingerMove(Finger finger)
    {
        if (!fingerRoles.TryGetValue(finger.index, out var role)) return;

        if (role == FingerRole.Menu)
        {
            gameplayMenuCanvas.HandleFingerMove(finger);
        }

        if (role == FingerRole.Move || role == FingerRole.Aim)
        {
            movAndAimCanvas.HandleFingerMove(finger);
        }
    }

    void HandleFingerUp(Finger finger)
    {
        if (!fingerRoles.TryGetValue(finger.index, out var role))
        {
            //Debug.LogWarning($"Finger {finger.index} no está registrado en fingerRoles al levantar el dedo.");
            return;
        }

        if (role == FingerRole.Move || role == FingerRole.Aim)
        {
            movAndAimCanvas.HandleFingerUp(finger);
        }

        if (role == FingerRole.Action)
        {
            PlayerActionsCanvas.HandleFingerUp(finger);
        }

        //Debug.Log($"Removing: {finger.index}");
        fingerRoles.Remove(finger.index);
        //Debug.Log($"Current Dictionary after change: {fingerRoles.Count} items");
    }

}
