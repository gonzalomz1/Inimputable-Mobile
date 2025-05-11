using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class MovAndAimCanvas : GameplayCanvas
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerData playerData;

    [SerializeField] private Animator movementAnimator;

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

    public bool HandleTouch(Finger finger, out FingerRole role)
    {
        // HandleFingerDown
        role = FingerRole.None;
        Debug.Log($"finger {finger.index}, is touching left side?: " + IsTouchingLeftSide(finger));
        if (IsTouchingLeftSide(finger) && movementFinger == null && finger != aimFinger)
        {
            Debug.Log($"Assigning {finger.index} to movementFinger");
            movementFinger = finger;
            movementAmount = Vector2.zero;
            joystickVisualLogic(finger);
            role = FingerRole.Move;
            return true;
        }
        else if (!IsTouchingLeftSide(finger) && aimFinger == null && finger != movementFinger)
        {
            Debug.Log($"Assigning {finger.index} to aimFinger");
            aimFinger = finger;
            role = FingerRole.Aim;
            return true;
        }
        return false;
    }

    public void HandleFingerMove(Finger finger)
    {
        if (finger == movementFinger)
        {
            MovementLogic(finger);
        }
        else if (finger == aimFinger)
        {
            AimWithFinger(finger);
        }
    }

    public void HandleFingerUp(Finger finger)
    {
        if (finger == movementFinger)
        {
            movementFinger = null;
            movementAmount = Vector2.zero;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
        }
        else if (finger == aimFinger)
        {
            aimFinger = null;
        }
    }


    private void Update()
    {
        playerData.currentMoveInput = movementAmount;
        if (movementAnimator != null) movementAnimator.SetBool("isMoving", IsPlayerMoving());
        playerView.Move(playerData.currentMoveInput, playerData.moveSpeed, playerView.cam.transform);
        playerData.currentPitch = Mathf.Lerp(playerData.currentPitch, targetPitch, Time.deltaTime * rotationSmoothSpeed);
        playerData.currentYaw = Mathf.LerpAngle(playerData.currentYaw, targetYaw, Time.deltaTime * rotationSmoothSpeed);

        playerView.RotateCamera(playerData.currentYaw, playerData.currentPitch);
    }

    private void joystickVisualLogic(Finger finger)
    {
        if (!joystick) return;
        joystick.gameObject.SetActive(true);
        joystick.RectTransform.sizeDelta = joystickSize;
        joystick.RectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
    }

    private void MovementLogic(Finger finger)
    {
        float maxMovement = joystickSize.x / 2f;
        Vector2 difference = finger.currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
        movementAmount = Vector2.ClampMagnitude(difference / maxMovement, 1f);
        joystick.Knob.anchoredPosition = movementAmount * maxMovement;
    }

    private void AimWithFinger(Finger finger)
    {
        Vector2 delta = finger.currentTouch.delta;



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

    private bool IsTouchingLeftSide(Finger finger)
    {
        Debug.Log($"screen width: {Screen.width}");
        Debug.Log($"finger.screenPosition.x: {finger.screenPosition}");
        return finger.screenPosition.x <= Screen.width / 2f;
    }

    private bool IsPlayerMoving()
    {
        if (playerData.currentMoveInput != Vector2.zero)
        {
            playerData.isMoving = true;
            return true;
        } else
        {
            playerData.isMoving = false;
            return false;
        }
    }
}