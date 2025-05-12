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

    [SerializeField] private Dictionary<int, FingerRole> fingerRoles = new Dictionary<int, FingerRole>();


    public void StartGameplay()
    {
        uICanvas.StartUIPlayerStats();
        EnableInput();
    }
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
        // Don't let have multiple fingers on an index
        if (fingerRoles.ContainsKey(finger.index))
        {
            Debug.LogWarning($"Finger {finger.index} ya está asignado a un rol ({fingerRoles[finger.index]}), ignorando nueva asignación.");
            return;
        }


        // Detect if try to touch an action button
        if (actionCanvas.HandleTouch(finger, out FingerRole role))
        {
            fingerRoles[finger.index] = role;
            Debug.Log($"Finger asignado a {role}.");
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
        if (!fingerRoles.TryGetValue(finger.index, out var role))
        {
            Debug.LogWarning($"Finger {finger.index} no está registrado en fingerRoles al levantar el dedo.");
            return;
        }

        if (role == FingerRole.Move || role == FingerRole.Aim)
        {
            movAndAimCanvas.HandleFingerUp(finger);
        }

        Debug.Log($"Removing: {finger.index}");
        fingerRoles.Remove(finger.index);
        Debug.Log($"Current Dictionary after change: {fingerRoles.Count} items");
    }
}
