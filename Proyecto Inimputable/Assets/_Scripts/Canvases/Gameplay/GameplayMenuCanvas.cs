
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class GameplayMenuCanvas : MonoBehaviour
{
    public static GameplayMenuCanvas instance;
    [Header("Canvas Manager")]
    [SerializeField] private CanvasManager canvasManager;
    
    [Header("Values")]
    [SerializeField] private Slider sensitivitySlider;
    private Vector2 startValueSensSldr;
    
    [Header("Shared Context")]
    [Tooltip("The parent container for both Pause and Game Over panels")]
    [SerializeField] private GameObject pausePanel; 
    
    [Header("Pause Context")]
    [SerializeField] private GameObject pauseContext;
    [SerializeField] private GameObject gameplayControls;

    [Header("Game Over Context")]
    [SerializeField] private GameObject gameOverContext;
    
    [Header("Fingers")]
    private Finger sensFinger;

    // ... (Existing code) ...

    public void SetLoseState()
    {
        SetActiveGameOver();
    }

    public void SetWinState()
    {
        SetActiveGameOver();
    }

    public void SetActiveGameOver()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        gameOverContext.SetActive(true);
        pauseContext.SetActive(false);
    }

    // New Button Methods for the duplicated panel
    public void OnRetryPressed()
    {
        Debug.Log("OnRetryPressed");
        if (GameManager.instance != null)
        {
            Debug.Log("Retry Game");
            GameManager.instance.RetryGame();
            gameObject.SetActive(false); 
        }
    }

    public void OnMenuPressed()
    {
        Debug.Log("OnMenuPressed");
        if (GameManager.instance != null)
        {
            Debug.Log("Quit to Menu");
            GameManager.instance.QuitToMenu();
        }
    }
    
    // Alias methods if referenced by animations or events (Optional, keeping clean)
    public void PlayAgain() => OnRetryPressed(); 
    public void GoToMenu() => OnMenuPressed();

    public void GameOver()
    {
        gameObject.SetActive(true);
        SetActiveGameOver();
    }

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    [Header("Player Reference")]
    public PlayerData playerData;

    public const float MIN_SENSITIVITY = 2f;
    public const float MAX_SENSITIVITY = 14f;
    public event Action ResumeGameplayRequest;
    public event Action ShowControlsRequest;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SubscribeToCanvasManagerEvents();
    }

    void SubscribeToCanvasManagerEvents()
    {
        canvasManager.RequestShowControlsToGameplayMenuCanvas += OnRequestShowControlsToGameplayMenuCanvas;
    }


    void Start()
    {
        SetSensitivitySlider();
    }

    private void SetSensitivitySlider()
    {
        sensitivitySlider.minValue = MIN_SENSITIVITY;
        sensitivitySlider.maxValue = MAX_SENSITIVITY;
        sensitivitySlider.value = playerData.sensitivityX;
    }

    public bool HandleTouch(Finger finger, out FingerRole assignedRole)
    {
        assignedRole = FingerRole.None;

        // Convert Finger.screenPosition on an event of raycast
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = finger.screenPosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            GameObject target = result.gameObject;
            if (target.GetComponent<Selectable>())
            {
                assignedRole = FingerRole.Menu;
                //Debug.Log("UI interactiva tocada: " + target.name);

                Slider slr = target.GetComponent<Slider>();
                if (slr != null)
                {
                    sensFinger = finger;
                    //Debug.Log("Se toco un slider");
                }
                // Si es botón, disparamos onClick
                Button btn = target.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke();
                }
                return true;
            }

        }
        //Debug.Log("On MenuCanvas.HandleTrouch(): returning false");
        return false;
    }

    public void HandleFingerMove(Finger finger)
    {
        if (sensFinger != null)
        {
            Vector2 currentPos = finger.screenPosition;
            Vector2 delta = currentPos - startValueSensSldr;
            startValueSensSldr = currentPos;

            Vector2 deltaNormalized = delta.normalized;

            float sensitivityChange = deltaNormalized.x / 12f;

            sensitivitySlider.value += sensitivityChange;

            if (sensitivitySlider.value <= MIN_SENSITIVITY) sensitivitySlider.value = MIN_SENSITIVITY;
            if (sensitivitySlider.value >= MAX_SENSITIVITY) sensitivitySlider.value = MAX_SENSITIVITY;

            if (playerData != null) playerData.ChangeSensitivity(sensitivitySlider.value);
        }
    }

    public void HandleFingerUp(Finger finger)
    {
        if (sensFinger != null)
        {
            sensFinger = null;
        }
    }


    
    public void SetActivePause()
    {
        gameObject.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(true);
        gameOverContext.SetActive(false);
        pauseContext.SetActive(true);
    }

    public void ExitMenu()
    {
        ResumeGameplayRequest?.Invoke();
    }

    public void ShowControls()
    {
        ShowControlsRequest?.Invoke();
    }

    void OnRequestShowControlsToGameplayMenuCanvas()
    {
        pausePanel.SetActive(false);
        gameplayControls.SetActive(true);

    }

    public void ExitFromGameplayControls()
    {
        pausePanel.SetActive(true);
        gameplayControls.SetActive(false);

    }
}
