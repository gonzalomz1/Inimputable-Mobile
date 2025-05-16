
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
            if (result.gameObject == playAgainButton.gameObject)
            {
                playAgainButton.onClick.Invoke();
                assignedRole = FingerRole.Menu;
                Debug.Log("On MenuCanvas.HandleTrouch(): returning true");
                return true;
            }
        }
        Debug.Log("On MenuCanvas.HandleTrouch(): returning false");
        return false;
    }

    public void SetLoseState()
    {
        loseText.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
    }

    public void SetWinState()
    {
        loseText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
