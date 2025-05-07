using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class CanvasManager : MonoBehaviour
{
    [Header("Gameplay Canvases")]
    [SerializeField] private MovAndAimCanvas movAndAimCanvas;
    [SerializeField] private UICanvas uICanvas;
    [SerializeField] private ActionCanvas actionCanvas;

    private Dictionary<int, FingerRole> fingerRoles = new Dictionary<int, FingerRole>();

    public void EnableInput()
    {
        ETouch.EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        ETouch.Touch.onFingerUp += HandleFingerUp;
    }

    public void DisableInput()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.EnhancedTouchSupport.Disable();
    }

    void HandleFingerDown(Finger finger)
    {
        // First detect if try to touch an action button
        if (actionCanvas.HandleTouch(finger, out FingerRole role))
        {
            fingerRoles[finger.index] = role;
            Debug.Log($"Finger {finger.index} asignado a {role}");
            return;
        }

        // Then try with move and aim
        if (movAndAimCanvas.HandleTouch(finger, out role))
        {
            fingerRoles[finger.index] = role;
            Debug.Log($"Finger {finger.index} asignado a {role}");
            return;
        }

        // If not action neither move and aim
        fingerRoles[finger.index] = FingerRole.None;
    }

    private void HandleFingerMove(Finger finger)
    {
        if (!fingerRoles.TryGetValue(finger.index, out var role)) return;

        if (role == FingerRole.Move || role == FingerRole.Aim)
        {
            movAndAimCanvas.HandleFingerMove(finger);
        }
    }

    void HandleFingerUp(Finger finger)
    {
        if (fingerRoles.ContainsKey(finger.index))
        {
            fingerRoles.Remove(finger.index);
        }
    }
}
