using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public interface IInputCanvas
{
    void EnableInput();
    void DisableInput();
}

public abstract class GameplayCanvas : CustomCanvas {


}