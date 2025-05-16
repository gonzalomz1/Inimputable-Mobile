using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class MovAndAimCanvas : GameplayCanvas
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick moveJoystick;
    [SerializeField] private RectTransform aimPanel;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerData playerData;

    [SerializeField] private Animator movementAnimator;

    private Finger movementFinger;
    private Finger aimFinger;
    private Vector2 lastAimPosition;
    private Vector2 movementAmount;

    private void Awake()
    {
        if (!playerData) playerData = GetComponent<PlayerData>();
        if (!playerView) playerView = GetComponent<PlayerView>();
        //playerData.cameraPitch = playerView.cam.localEulerAngles.x;
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
            joystickVisualLogic(finger, moveJoystick);
            role = FingerRole.Move;
            return true;
        }
        else if (!IsTouchingLeftSide(finger) && aimFinger == null && finger != movementFinger)
        {
            Debug.Log($"Assigning {finger.index} to aimFinger");
            aimFinger = finger;
            lastAimPosition = finger.screenPosition;
            role = FingerRole.Aim;
            return true;
        }
        return false;
    }

    public void HandleFingerMove(Finger finger)
    {
        if (finger == movementFinger)
        {
            JoystickVectorLogic(finger, moveJoystick);
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
            moveJoystick.Knob.anchoredPosition = Vector2.zero;
            moveJoystick.gameObject.SetActive(false);
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

        playerView.Move(playerData.currentMoveInput, playerData.moveSpeed);
        playerView.RotateCamera(playerData.aimX, playerData.aimY);
    }

    private void joystickVisualLogic(Finger finger, FloatingJoystick joystick)
    {
        if (!joystick) return;
        joystick.gameObject.SetActive(true);
        joystick.RectTransform.sizeDelta = joystickSize;
        joystick.RectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
    }

    private void JoystickVectorLogic(Finger finger, FloatingJoystick joystick)
    {
        float maxMovement = joystickSize.x / 2f;
        Vector2 difference = finger.currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
        movementAmount = Vector2.ClampMagnitude(difference / maxMovement, 1f);
        joystick.Knob.anchoredPosition = movementAmount * maxMovement;
    }
    private void AimWithFinger(Finger finger)
    {
        if (playerData.isAimAssistActive) return;
        Vector2 currentPos = finger.screenPosition;
        Vector2 delta = currentPos - lastAimPosition;
        lastAimPosition = currentPos;

        float deltaYaw = delta.x * playerData.sensitivity * Time.deltaTime;
        float deltaPitch = -delta.y * playerData.sensitivity * Time.deltaTime;

        playerData.aimX += deltaYaw;
        playerData.aimY += deltaPitch;

        // Clampeo del ángulo vertical si lo necesitás
        playerData.aimY = Mathf.Clamp(playerData.aimY, -80f, 80f);
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
        }
        else
        {
            playerData.isMoving = false;
            return false;
        }
    }
}