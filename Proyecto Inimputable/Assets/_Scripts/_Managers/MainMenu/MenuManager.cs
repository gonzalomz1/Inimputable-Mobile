using System;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;


public class MenuManager : MonoBehaviour
{
   [Header("State")]
   public MenuFlowState currentState;
   public MenuState currentMenuState;
   [Header("Components")]
   public Basement basement;
   public CameraController cameraController;
   public HandsController handsController;
   [Header("UI References")]
   public SpriteRenderer title;
   public RectTransform menuScreenCanvasRect;
   public MenuScreen menuScreen;
   public SplashScreen splashScreen;
   public ScreenFader screenFader;
   [Header("RenderTexture UI")]
   public RectTransform rawImageRect; // el RectTransform de la RawImage que muestra la RenderTexture

   [Header("Raycaster")]
   public GraphicRaycaster raycaster;
   [Header("Event Object")]
   public EventSystem eventSystem;
   private Finger menuFinger;

   void Start()
   {
      StartGame();
   }

   void StartGame()
   {
      screenFader.gameObject.SetActive(false);
      currentState = MenuFlowState.Logo;
      currentMenuState = MenuState.Disable;
      ManageState(currentState);
      ManageMenuState(currentMenuState);
   }


   // INPUT    
   void EnableInput()
   {
      ETouch.EnhancedTouchSupport.Enable();
      ETouch.Touch.onFingerDown += HandleFingerDown;
   }

   void DisableInput()
   {
      ETouch.Touch.onFingerDown -= HandleFingerDown;
      ETouch.EnhancedTouchSupport.Disable();
   }

   void HandleFingerDown(Finger finger)
   {
    if (currentState != MenuFlowState.Menu) return;

    Vector2 touchPos = finger.screenPosition;
    PointerEventData pointerEventData = new PointerEventData(eventSystem);
    pointerEventData.position = touchPos;

    var results = new List<RaycastResult>();
    raycaster.Raycast(pointerEventData, results);

    foreach (var result in results)
    {
        Button button = result.gameObject.GetComponent<Button>();
        if (button != null)
        {
            Debug.Log("Botón tocado: " + button.name);
            button.onClick.Invoke(); // Simula el click
            break;
        }
    }
   }
   // END OF INPUT CONTEXT

   // STATE MACHINES
   void ManageState(MenuFlowState current)
   {
      switch (current)
      {
         case MenuFlowState.Logo:
            SetSplashScreenLogoMode();
            break;
         case MenuFlowState.TvClip:
            SetCameraTvAngle();
            PlayInimputableClip();
            break;
         case MenuFlowState.Menu:
            SetCameraMainMenuAngle();
            break;
         case MenuFlowState.DrinkingBottle:
            DisableInput();
            HideTitle();
            HideBottleInWorld();
            menuScreen.SetActiveCanvas(false);
            handsController.ControllerPlayState();
            break;
         case MenuFlowState.LoadGameplay:
            splashScreen.SetActiveCanvas(true);
            splashScreen.DisableCredits();
            splashScreen.EnableLoading();
            splashScreen.LoadGameplay();
            break;
      }
   }
   public void ManageMenuState(MenuState current)
   {
      switch (current)
      {
         case MenuState.Disable:
            SetCanvasState(menuScreen, false);
            break;
         case MenuState.MainMenu:
            DisableAllContextExceptMainMenu();
            EnableInput();
            SetCanvasState(menuScreen, true);
            
            MainMenuButtons(true);
            break;
         case MenuState.Credits:
            cameraController.SetCreditsAngle();
            menuScreen.HideMainMenuContext();
            break;
         case MenuState.Options:
            menuScreen.SetGameObject(menuScreen.optionsContext, true);
            menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
            break;
         case MenuState.Controls:
            DisableAllContextExceptControls();
            break;
         case MenuState.Exit:
            menuScreen.SetGameObject(menuScreen.exitContext, true);
            menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
            menuScreen.SetGameObject(menuScreen.optionsContext, false);
            menuScreen.SetGameObject(menuScreen.creditsContext, false);
            break;

      }

   }
   // END OF STATE MACHINE CONTEXT

   void SetSplashScreenLogoMode()
   {
      splashScreen.gameObject.SetActive(true);
      splashScreen.DisableLoading();
      splashScreen.EnableCredits();
      splashScreen.PlayLogoAnimation();
   }

   void PlayInimputableClip()
   {
      basement.tv.tvScreen.PlayVideo();
   }
   void SetCameraTvAngle()
   {
      cameraController.SetTvClipAngle();
   }

   public void SetCameraMainMenuAngle()
   {
      cameraController.SetMainMenuAngle();
   }

   public void FromCreditsToMainMenu()
   {
      cameraController.FromCreditsToMainMenu();
   }

   public void DisableAllContexts()
   {
      menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
      menuScreen.SetGameObject(menuScreen.creditsContext, false);
      menuScreen.SetGameObject(menuScreen.optionsContext, false);
      menuScreen.SetGameObject(menuScreen.controlsContext, false);
      menuScreen.SetGameObject(menuScreen.exitContext, false);
   }

   public void DisableAllContextExceptMainMenu()
   {
      menuScreen.SetGameObject(menuScreen.mainMenuContext, true);
      menuScreen.SetGameObject(menuScreen.creditsContext, false);
      menuScreen.SetGameObject(menuScreen.optionsContext, false);
      menuScreen.SetGameObject(menuScreen.controlsContext, false);
      menuScreen.SetGameObject(menuScreen.exitContext, false);
   }

   public void DisableAllContextExceptControls()
   {
      menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
      menuScreen.SetGameObject(menuScreen.creditsContext, false);
      menuScreen.SetGameObject(menuScreen.optionsContext, false);
      menuScreen.SetGameObject(menuScreen.controlsContext, true);
      menuScreen.SetGameObject(menuScreen.exitContext, false);
   }

   public void OnlyCreditsButtons()
   {
      menuScreen.SetGameObject(menuScreen.creditsContext, true);
      menuScreen.CreditsArrow(true);
      menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
      menuScreen.SetGameObject(menuScreen.optionsContext, false);
      MainMenuButtons(false);
   }

   private void MainMenuButtons(bool ActiveOrNot)
   {
      menuScreen.MainMenuButtons(ActiveOrNot);
   }

   private void SetCanvasState(CustomCanvas screen, bool boolean)
   {
      screen.SetActiveCanvas(boolean);
   }

   // METHODS FOR EVENT CALLS IN ANIMATIONS.
   public void OnLogoAnimationFinished()
   {
      Debug.Log("termino animacion del logo");
      splashScreen.gameObject.SetActive(false);
      currentState = MenuFlowState.TvClip;
      ManageState(currentState);
   }
   public void OnVideoEnd()
   {
      currentState = MenuFlowState.Menu;
      ManageState(currentState);
   }


// called from CameraController.cs
   public void SetMenuStateToMainMenu()
   {
      currentMenuState = MenuState.MainMenu;
      ManageMenuState(currentMenuState);
   }

   public void StartDrinkingBottle()
   {
      currentState = MenuFlowState.DrinkingBottle;
      ManageState(currentState);
   }

   public void BeginLoadingGameplay()
   {
      currentState = MenuFlowState.LoadGameplay;
      ManageState(currentState);
   }

   private void HideTitle()
   {
      title.gameObject.SetActive(false);
   }

   private void HideBottleInWorld()
   {
      basement.HideBottle();
   }

   public void CameraDrinkingAngle()
   {
      cameraController.GetComponent<Animator>().SetTrigger("DrinkBottle");
   }
}
