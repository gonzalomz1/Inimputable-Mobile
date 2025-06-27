using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private HandsView handsView;
    [SerializeField] private ScreenFader screenFader;

    public void ControllerMainMenuState()
    {
        handsView.SetMainMenuState();
    }

    public void ControllerPlayState()
    {
        handsView.SetPlayButtonState();
    }

    public void ControllerDrinkAnimationStarted()
    {
        menuManager.CameraDrinkingAngle();
    }

    public void ControllerDrinkAnimationFinished()
    {
        menuManager.BeginLoadingGameplay();
    }

    public void FadeIn()
    {
        screenFader.FadeIn();
    }
}
