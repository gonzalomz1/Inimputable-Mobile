using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : CustomCanvas

{
    public MenuManager menuManager;
    [Header("Splash Screen")]
    public SplashScreen splashScreen;
    [Header("Main Menu Buttons")]
    public Button mainMenuPlay;
    public Button mainMenuOptions;
    public Button mainMenuCredits;
    public Button mainMenuControls;
    public Button mainMenuExit;
    
    [Header("Options Button")]
    public Button optionsBrightness;
    public Button optionsExit;

    [Header("Credits Button")]
    public Button creditsExit;
    [Header("Controls Button")]
    public Button controlsExit;

    [Header("Exit")]
    public Button exitExit;

    [Header("Contexts")]
    public GameObject creditsContext;

    public GameObject mainMenuContext;

    public GameObject optionsContext;

    public GameObject controlsContext;

    public GameObject exitContext;

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }

    public void Play()
    {
        menuManager.StartDrinkingBottle();
    }

    public void Options()
    {

    }

    public void Credits()
    {
        menuManager.currentMenuState = MenuState.Credits;
        menuManager.ManageMenuState(menuManager.currentMenuState);
    }

    public void Controls()
    {
        menuManager.currentMenuState = MenuState.Controls;
        menuManager.ManageMenuState(menuManager.currentMenuState);
    }

    public void Exit()
    {
        menuManager.currentMenuState = MenuState.Exit;
        menuManager.ManageMenuState(menuManager.currentMenuState);
    }

    public void SetGameObject(GameObject gameObject, bool boolean)
    {
        gameObject.SetActive(boolean);
    }

    public void CreditsArrow(bool boolean)
    {
        creditsExit.gameObject.SetActive(boolean);
    }
    public void ReturnToMainMenu()
    {
        menuManager.FromCreditsToMainMenu();
        menuManager.DisableAllContexts();
    }

    public void MainMenuButtons(bool boolean)
    {
        mainMenuExit.gameObject.SetActive(boolean);
        mainMenuCredits.gameObject.SetActive(boolean);
        mainMenuOptions.gameObject.SetActive(boolean);
        mainMenuPlay.gameObject.SetActive(boolean);

    }
    public void OnExitButtonNo()
    {
        menuManager.currentMenuState = MenuState.MainMenu;
        menuManager.ManageMenuState(menuManager.currentMenuState);
    }

    public void HideMainMenuContext()
    {
        mainMenuContext.gameObject.SetActive(false);
    }
    
    public void ShowMainMenuContext()
    {
        mainMenuContext.gameObject.SetActive(true);
    }
}
