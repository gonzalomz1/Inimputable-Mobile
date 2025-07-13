using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraView cameraView;

    public event Action OnMenuAnimationFinished;
    public event Action OnCreditsAnimationFinished;
    public event Action OnFromCreditsToMainMenuAnimationFinished;

    public void SetTvClipAngle() => cameraView.SetTvClipAngle();
    public void SetMainMenuAngle() => cameraView.SetMainMenuAngle();
    public void SetCreditsAngle() => cameraView.SetCreditsAngle();
    public void FromCreditsToMainMenu() => cameraView.FromCreditsToMainMenu();

    // callbacks from animations
    public void MenuAnimationFinished() => OnMenuAnimationFinished?.Invoke();
    public void CreditsAnimationFinished() => OnCreditsAnimationFinished?.Invoke();
    public void FromCreditsToMainMenuAnimationFinished() => OnFromCreditsToMainMenuAnimationFinished?.Invoke();
}
