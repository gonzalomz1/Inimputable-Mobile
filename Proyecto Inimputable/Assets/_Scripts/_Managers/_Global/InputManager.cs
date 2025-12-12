using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using System;
using System.Collections.Generic;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Action<Finger> OnFingerDown;
    public Action<Finger> OnFingerMove;
    public Action<Finger> OnFingerUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ETouch.EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    private void OnEnable()
    {
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        ETouch.Touch.onFingerUp += HandleFingerUp;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            TouchSimulation.Disable();
            ETouch.EnhancedTouchSupport.Disable();
        }
    }
    
    private void HandleFingerDown(Finger finger) => OnFingerDown?.Invoke(finger);
    private void HandleFingerMove(Finger finger) => OnFingerMove?.Invoke(finger);
    private void HandleFingerUp(Finger finger) => OnFingerUp?.Invoke(finger);
}
