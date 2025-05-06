using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class MovAndAimCanvas : GameplayCanvas, IInputCanvas
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerData playerData;

    private Finger movementFinger;
    private Finger aimFinger;
    private Vector2 movementAmount;

    [SerializeField] private float deadZoneThreshold = 5f;
    [SerializeField] private float rotationSmoothSpeed = 10f;

    private float targetYaw;
    private float targetPitch;

    private void Awake()
    {
        if (!playerData) playerData = GetComponent<PlayerData>();
        if (!playerView) playerView = GetComponent<PlayerView>();
        playerData.cameraPitch = playerView.cameraPivot.localEulerAngles.x;
    }

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


    public void EnableInput()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }
    public void DisableInput()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();

        if (movementFinger != null)
        {
            canvasManager.UnregisterFinger(movementFinger);
            movementFinger = null;
        }

        if (aimFinger != null)
        {
            canvasManager.UnregisterFinger(aimFinger);
            aimFinger = null;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {

        if (canvasManager.IsFingerInUse(touchedFinger)) return;
            
        if (movementFinger == null && IsTouchingLeftSide(touchedFinger))
        {
            if (canvasManager.TryRegisterFinger(touchedFinger, FingerRole.Move))
            {
                movementFinger = touchedFinger;
                movementAmount = Vector2.zero;
                joystickVisualLogic(touchedFinger);
            }
        }
        else if (aimFinger == null && !IsTouchingLeftSide(touchedFinger))
        {
            if (canvasManager.TryRegisterFinger(touchedFinger, FingerRole.Aim))
            {
                aimFinger = touchedFinger;
            }
        }
    }


    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            MovementLogic(movedFinger);
        }
        else if (movedFinger == aimFinger)
        {
            if (IsTouchingLeftSide(movedFinger)) return;
            AimWithFinger(movedFinger);
        }
    }

        private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            canvasManager.UnregisterFinger(movementFinger);
            movementFinger = null;
            movementAmount = Vector2.zero;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
        }
        else if (lostFinger == aimFinger)
        {
            canvasManager.UnregisterFinger(aimFinger);
            aimFinger = null;
        }
    }


    public void FingerDownIsMovementFinger(Finger touchedFinger)
    {
        movementFinger = touchedFinger;
        movementAmount = Vector2.zero;
        joystickVisualLogic(touchedFinger);
    }

    public void joystickVisualLogic(Finger movementFinger)
    {
        joystick.gameObject.SetActive(true);
        joystick.RectTransform.sizeDelta = joystickSize;
        joystick.RectTransform.anchoredPosition = ClampStartPosition(movementFinger.screenPosition);
    }

    public void MovementLogic(Finger movedFinger)
    {
        float maxMovement = joystickSize.x / 2f;
        Vector2 difference = movedFinger.currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
        movementAmount = Vector2.ClampMagnitude(difference / maxMovement, 1f);
        joystick.Knob.anchoredPosition = movementAmount * maxMovement;
    }


    private void AimWithFinger(Finger movedFinger)
    {
        Vector2 delta = movedFinger.currentTouch.delta;

        if (float.IsNaN(delta.x) || float.IsNaN(delta.y)) return;

        if (Mathf.Abs(delta.x) < deadZoneThreshold) delta.x = 0f;
        if (Mathf.Abs(delta.y) < deadZoneThreshold) delta.y = 0f;

        if (delta != Vector2.zero)
        {
            targetYaw += delta.x * playerData.lookSensitivity;
            targetPitch -= delta.y * playerData.lookSensitivity;
            targetPitch = Mathf.Clamp(targetPitch, -80f, 80f);
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        startPosition.x = Mathf.Max(startPosition.x, joystickSize.x / 2);
        startPosition.y = Mathf.Clamp(startPosition.y, joystickSize.y / 2, Screen.height - joystickSize.y / 2);
        return startPosition;
    }
    private bool IsTouchingLeftSide(Finger currentFinger)
    {
        return currentFinger.screenPosition.x <= Screen.width / 2f;
    }


    private void Update()
    {
        playerData.currentMoveInput = movementAmount;
        playerView.Move(playerData.currentMoveInput, playerData.moveSpeed, playerView.cam.transform);

        playerData.currentPitch = Mathf.Lerp(playerData.currentPitch, targetPitch, Time.deltaTime * rotationSmoothSpeed);
        playerData.currentYaw = Mathf.LerpAngle(playerData.currentYaw, targetYaw, Time.deltaTime * rotationSmoothSpeed);

        playerView.RotateCamera(playerData.currentYaw, playerData.currentPitch);
    }
}