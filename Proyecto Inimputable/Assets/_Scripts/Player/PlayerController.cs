using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerView playerView;

    private Finger movementFinger;
    private Finger aimFinger;
    private Vector2 movementAmount;

    private void Awake()
    {
        if (!playerModel) playerModel = GetComponent<PlayerModel>();
        if (!playerView) playerView = GetComponent<PlayerView>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;

            joystick.gameObject.SetActive(true);
            joystick.RectTransform.sizeDelta = joystickSize;
            joystick.RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            float maxMovement = joystickSize.x / 2f;
            Vector2 difference = movedFinger.currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;

            movementAmount = Vector2.ClampMagnitude(difference / maxMovement, 1f);

            joystick.Knob.anchoredPosition = movementAmount * maxMovement;
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            movementAmount = Vector2.zero;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        startPosition.x = Mathf.Max(startPosition.x, joystickSize.x / 2);
        startPosition.y = Mathf.Clamp(startPosition.y, joystickSize.y / 2, Screen.height - joystickSize.y / 2);
        return startPosition;
    }

    private void Update()
    {
        // Guardar input en el modelo
        playerModel.currentMoveInput = movementAmount;

        // Pasar el input a la vista (movimiento)
        playerView.Move(playerModel.currentMoveInput, playerModel.moveSpeed);
    }
}