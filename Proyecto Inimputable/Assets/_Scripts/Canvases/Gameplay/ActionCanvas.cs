using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;


public class ActionCanvas : MonoBehaviour
{
    public Button shootButton;
    public Button reloadButton;
    public Button pauseButton;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public WeaponController weaponController;

    [SerializeField] private List<Sprite> shootSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> reloadSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> pauseSprites = new List<Sprite>();

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
                assignedRole = FingerRole.Action;
                if (button == shootButton) button.image.sprite = shootSprites[1];
                if (button == reloadButton) button.image.sprite = reloadSprites[1];
                if (button == pauseButton) button.image.sprite = pauseSprites[1];
                button.onClick.Invoke(); // Simula el click
                return true;
            }
        }
        Debug.Log("On ActionCanvas.HandleTrouch(): returning false");
        return false;
    }

    public void HandleFingerUp(Finger finger)
    {
        if (shootButton.image.sprite == shootSprites[1]) shootButton.image.sprite = shootSprites[0];
        if (reloadButton.image.sprite == reloadSprites[1]) reloadButton.image.sprite = reloadSprites[0];
        if (pauseButton.image.sprite == pauseSprites[1]) pauseButton.image.sprite = pauseSprites[0];
    }

    public void OnClickReload()
    {
        weaponController.TryReload();
    }
    public void OnClickShoot()
    {
        weaponController.TryShoot();
    }
    public void OnClickPause()
    {
        GetComponentInParent<GameFlowManager>().SetGameState(GameFlowState.Paused);
    }

    public bool IfNeedToChangeButtonSpray()
    {
        return true;
    }

}
