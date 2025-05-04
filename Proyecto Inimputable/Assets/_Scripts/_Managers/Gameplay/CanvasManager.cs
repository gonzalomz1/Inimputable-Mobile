using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class CanvasManager : GameplayCanvas
{
    [SerializeField] private MovAndAimCanvas movAndAimCanvas;
    [SerializeField] private UICanvas uICanvas;
    [SerializeField] private ActionCanvas actionCanvas;
    [SerializeField] private WeaponCanvas weaponCanvas;

    public enum FingerRole
    {
        Movement,
        Aim,
        Action,
        Weapon
    }
    
    private Dictionary<Finger, FingerRole> activeFingers = new Dictionary<Finger, FingerRole>();


    void Awake()
    {
        if (!movAndAimCanvas) GetComponent<MovAndAimCanvas>();
        if (!uICanvas) GetComponent<UICanvas>();
        if (!actionCanvas) GetComponent<ActionCanvas>();
        if (!weaponCanvas) GetComponent<WeaponCanvas>();
    }

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public bool TryRegisterFinger(Finger finger, FingerRole role)
    {
        if (activeFingers.ContainsKey(finger))
            return false; // Ya está siendo usado

        activeFingers.Add(finger, role);
        return true;
    }

    public void UnregisterFinger(Finger finger)
    {
        if (activeFingers.ContainsKey(finger))
            activeFingers.Remove(finger);
    }

    public bool IsFingerInUse(Finger finger)
    {
        return activeFingers.ContainsKey(finger);
    }

    public bool IsRoleActive(FingerRole role)
    {
        return activeFingers.ContainsValue(role);
    }


    public void StartGameplay()
    {
        DisableEverything();
        EnableMovAndAimCanvas();
        EnableWeaponCanvas();
        StartInput();
        StartDefaultUI();
        EnableActionCanvas();
    }

    void DisableEverything()
    {
        movAndAimCanvas.gameObject.SetActive(false);
        uICanvas.gameObject.SetActive(false);
        actionCanvas.gameObject.SetActive(false);
        weaponCanvas.gameObject.SetActive(false);
    }

    public void StartInput()
    {
        movAndAimCanvas.EnableInput();
    }

    public void TurnOffInput()
    {
        movAndAimCanvas.DisableInput();
    }

    void EnableMovAndAimCanvas()
    {
        movAndAimCanvas.gameObject.SetActive(true);
    }

    void EnableWeaponCanvas()
    {
        weaponCanvas.gameObject.SetActive(true);
    }


    void StartDefaultUI()
    {

    }

    void EnableActionCanvas()
    {
        actionCanvas.gameObject.SetActive(true);
    }

}
