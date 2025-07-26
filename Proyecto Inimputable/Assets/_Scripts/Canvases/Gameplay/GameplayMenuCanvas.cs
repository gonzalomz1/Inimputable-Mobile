
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
    public static GameplayMenuCanvas Instance { get; private set; }
    [Header("Canvas Manager")]
    [SerializeField] private CanvasManager canvasManager;
    [Header("Pause")]
    [SerializeField] private Slider sensitivitySlider;
    private Vector2 startValueSensSldr;
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject exitButton;
    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI winText;
    public Button playAgainButton;
    [Header("Contexts")]
    [SerializeField] private GameObject gameplayControls;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverContext;
    [SerializeField] private GameObject pauseContext;
    [Header("Fingers")]
    private Finger sensFinger;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SubscribeToCanvasManagerEvents();
    }

    void SubscribeToCanvasManagerEvents()
    {
        canvasManager.RequestShowControlsToGameplayMenuCanvas += OnRequestShowControlsToGameplayMenuCanvas;
    }





    void Start()
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



    public void SetLoseState()
    {
        SetActiveGameOver();

        loseText.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
    }

    public void SetWinState()
    {
        SetActiveGameOver();

        loseText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
    }

    public void SetActiveGameOver()
    {
        gameOverContext.SetActive(true);
        pauseContext.SetActive(false);
    }

    public void SetActivePause()
    {
        gameObject.SetActive(true);
        gameOverContext.SetActive(false);
        pauseContext.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
        SetActiveGameOver();
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
