using System;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private HandsView handsView;
    [SerializeField] private ScreenFader screenFader;

    public event Action DrinkBottle;
    public event Action DrinkAnimationStarted;
    public event Action DrinkAnimationFinished;

    void Start()
    {
        GameManager.instance.GameplayStart += OnGameplayStart;
        GameManager.instance.GameplayExit += OnGameplayExit;
    }

    public void ControllerMainMenuState()
    {
        handsView.SetMainMenuState();
    }

    public void ControllerDrinkingToStartGameplayState()
    {
        handsView.SetPlayButtonState();
    }

    public void ControllerDrinkAnimationStarted()
    {
        DrinkAnimationStarted?.Invoke();
    }

    public void ControllerDrinkAnimationFinished()
    {
        DrinkAnimationFinished?.Invoke();
    }

    public void AnimationEventDrinkBottle()
    {
        DrinkBottle?.Invoke();
    }

    private void OnGameplayStart()
    {
        GameplayMode();
    }

    private void OnGameplayExit()
    {
        MainMenuMode();
    }

    public void GameplayMode()
    {
        gameObject.SetActive(false);
    }

    private void MainMenuMode()
    {
        gameObject.SetActive(true);
    }
}
