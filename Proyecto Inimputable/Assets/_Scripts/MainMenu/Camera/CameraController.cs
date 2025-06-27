using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraView cameraView;
    [SerializeField] private MenuManager menuManager;

    public void SetTvClipAngle()
    {
        cameraView.SetTvClipAngle();
    }
    public void SetMainMenuAngle()
    {
        cameraView.SetMainMenuAngle();
    }
    public void SetCreditsAngle()
    {
        cameraView.SetCreditsAngle();
    }
    public void FromCreditsToMainMenu()
    {
        cameraView.FromCreditsToMainMenu();
    }

    public void OnMenuAnimationFinished()
    {
        menuManager.SetMenuStateToMainMenu();
    }

    public void OnCreditsAnimationFinished()
    {
        menuManager.OnlyCreditsButtons();
    }

    public void OnFromCreditsToMainMenuAnimationFinished()
    {
        menuManager.SetMenuStateToMainMenu();
    }
}
