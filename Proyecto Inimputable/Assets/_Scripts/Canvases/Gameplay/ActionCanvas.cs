using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;


public class ActionCanvas : MonoBehaviour
{
    public Button shootButton;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public WeaponController weaponController; 

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
            if (result.gameObject == shootButton.gameObject)
            {
                weaponController.TryShoot();
                assignedRole = FingerRole.Action;
                return true;
            }
        }
        return false;
    }
}
