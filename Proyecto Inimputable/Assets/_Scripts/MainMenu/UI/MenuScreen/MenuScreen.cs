using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : CustomCanvas

{
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

    public event Action OnPlayPressed;
    public event Action OnOptionsPressed;
    public event Action OnCreditsPressed;
    public event Action OnFromCreditsToMainMenuPressed;
    public event Action OnControlsPressed;
    public event Action OnControlsExitPressed;
    public event Action OnExitPressed;
    public event Action OnExitContextNoPressed;
    public event Action OnExitContextYesPressed;

    void Start()
    {
        mainMenuPlay.onClick.AddListener(() => OnPlayPressed?.Invoke());
        mainMenuOptions.onClick.AddListener(() => OnOptionsPressed?.Invoke());
        mainMenuCredits.onClick.AddListener(() => OnCreditsPressed?.Invoke());
        mainMenuControls.onClick.AddListener(() => OnControlsPressed?.Invoke());
        mainMenuExit.onClick.AddListener(() => OnExitPressed?.Invoke());
        creditsExit.onClick.AddListener(() => OnFromCreditsToMainMenuPressed?.Invoke());
        controlsExit.onClick.AddListener(() => OnControlsExitPressed?.Invoke());
    //exitExit.onClick.AddListener(() => OnExitContextYesPressed?.Invoke());
    }


    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }


    public void SetGameObject(GameObject gameObject, bool boolean)
    {
        gameObject.SetActive(boolean);
    }

    public void CreditsArrow(bool boolean)
    {
        creditsExit.gameObject.SetActive(boolean);
    }

    public void MainMenuButtons(bool boolean)
    {
        mainMenuExit.gameObject.SetActive(boolean);
        mainMenuCredits.gameObject.SetActive(boolean);
        mainMenuOptions.gameObject.SetActive(boolean);
        mainMenuPlay.gameObject.SetActive(boolean);
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
