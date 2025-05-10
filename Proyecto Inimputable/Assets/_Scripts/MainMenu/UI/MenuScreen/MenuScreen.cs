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
    [Header("Main Menu Buttons")]
    public Button mainMenuPlay;
    public Button mainMenuOptions;
    public Button mainMenuCredits;
    public Button mainMenuExit;

    [Header("Credits Button")]
    public Button creditsExit;

    [Header("Options Button")]

    public Button optionsBrightness;

    public Button optionsExit;
    [Header("Exit")]
    public Button exitExit;

    [Header("Contexts")]
    public GameObject creditsContext;

    public GameObject mainMenuContext;

    public GameObject optionsContext;

    public GameObject exitContext;
    [Header("")]
    public MenuCamera menuCamera;



    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }

    public void SetTextMeshTransform(List<Vector2> list)
    {
        mainMenuPlay.transform.position = list[0];
        mainMenuOptions.transform.position = list[1];
        mainMenuCredits.transform.position = list[2];
        mainMenuExit.transform.position = list[3];
        menuManager.AfterTextPositionSet();
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex
         + 1);
    }

    public void Options()
    {

    }

    public void Credits()
    {
        menuManager.currentMenuState = MenuState.Credits;
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
    public void ReturnToMainMenu(){
    menuManager.currentMenuState = MenuState.MainMenu;
    menuManager.ManageMenuState(menuManager.currentMenuState);
    menuCamera.CreditsReturnAngle();
    }

    public void MainMenuButtons(bool boolean){
          mainMenuExit.gameObject.SetActive(boolean);
          mainMenuCredits.gameObject.SetActive(boolean);
          mainMenuOptions.gameObject.SetActive(boolean);
          mainMenuPlay.gameObject.SetActive(boolean);

    }
    public void OnExitButtonNo(){
       menuManager.currentMenuState = MenuState.MainMenu;
       menuManager.ManageMenuState(menuManager.currentMenuState);
    }
}
