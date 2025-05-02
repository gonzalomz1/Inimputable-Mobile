using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class ActionCanvas : GameplayCanvas, IInputCanvas
{
    public Finger[] ActiveFingers { get;}

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void EnableInput(){}
    public void DisableInput(){}
}
