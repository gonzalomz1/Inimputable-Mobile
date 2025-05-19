
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject gameOverContext;
    [SerializeField] private GameObject pauseContext;
    public Button playAgainButton;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

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
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                Debug.Log("Botón tocado: " + button.name);
                assignedRole = FingerRole.Menu;
                button.onClick.Invoke(); // Simula el click
                return true;
            }
        }
        Debug.Log("On MenuCanvas.HandleTrouch(): returning false");
        return false;
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
        GetComponentInParent<GameFlowManager>().SetGameState(GameFlowState.ResumeGameplay);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
