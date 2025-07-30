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
   public MenuFlowState currentMenuFlowState { get; private set; }
   public MenuState currentMenuState { get; private set; }

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

   public event Action InteractionSoundMenu;
   public event Action MenuLoopSongRequest;
   public event Action StartGameplay;
   public event Action CanvasMenuDisabled;
   public event Action ActivateInputs;
   public event Action InGameplayControls;

   void Awake()
   {
      SubscribeToGameManagerEvents();
      SubscribeToCameraControllerEvents();
      SubscribeToMenuScreenEvents();
      SubscribeToHandsControllerEvents();
   }

   void SubscribeToGameManagerEvents()
   {
      GameManager.instance.GameExecute += OnGameExecute;
      GameManager.instance.GameplayStart += OnGameplayStart;
   }

   void SubscribeToCameraControllerEvents()
   {
      cameraController.OnMenuAnimationFinished += SetMenuStateToMainMenuAndStartMenuLoopSong;
      cameraController.OnCreditsAnimationFinished += ShowCreditsArrow;
      cameraController.OnFromCreditsToMainMenuAnimationFinished += SetMenuStateToMainMenuWithoutSound;
   }

   void SubscribeToMenuScreenEvents()
   {
      menuScreen.OnPlayPressed += StartDrinkingBottle;
      menuScreen.OnOptionsPressed += ShowOptions;
      menuScreen.OnCreditsPressed += ShowCredits;
      menuScreen.OnControlsPressed += ShowControls;
      menuScreen.OnControlsExitPressed += SetMenuStateToMainMenuWithSound;
      menuScreen.OnFromCreditsToMainMenuPressed += ReturnToMainMenuFromCredits;
   }

   void SubscribeToHandsControllerEvents()
   {
      handsController.DrinkBottle += StartFadeIn;
      handsController.DrinkAnimationStarted += OnHandsControllerDrinkAnimationStarted;
      handsController.DrinkAnimationFinished += OnHandsControllerDrinkAnimationFinished;
   }

   // INPUT    

   public bool HandleTouch(Finger finger, out FingerRole role)
   {
      role = FingerRole.None;

      PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
      pointerEventData.position = finger.screenPosition;

      List<RaycastResult> results = new List<RaycastResult>();
      raycaster.Raycast(pointerEventData, results);

      foreach (var result in results)
      {
         Button button = result.gameObject.GetComponent<Button>();
         if (button != null)
         {
            button.onClick.Invoke();
            role = FingerRole.Menu;
            return true;
         }
      }

      return false;
   }



   // END OF INPUT CONTEXT

   // STATE MACHINES
   void ManageState(MenuFlowState current)
   {
      switch (current)
      {
         case MenuFlowState.Disabled:
            DisableAllMainMenuStuff();
            break;
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
            menuScreen.SetActiveCanvas(false);  
            HideTitle();
            HideBottleInWorld();
            handsController.ControllerDrinkingToStartGameplayState();
            GameManager.instance.DisableInput();
            break;
         case MenuFlowState.StartGameplay:
            StartGameplay.Invoke();
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
            GameManager.instance.EnableInput();
            SetCanvasState(menuScreen, true);
            MainMenuButtons(true);
            break;
         case MenuState.Credits:
            cameraController.SetCreditsAngle();
            DisableAllContextExceptCredits();
            HideCreditsArrow(); // we wait for animation call to show
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

   private void OnGameExecute()
   {
      FromExecuteGameStart();
   }

   public void FromExecuteGameStart()
   {
      print("MainMenu: from execute called.");
      currentMenuFlowState = MenuFlowState.Logo;
      currentMenuState = MenuState.Disable;
      ManageState(currentMenuFlowState);
      ManageMenuState(currentMenuState);
   }

   void SetSplashScreenLogoMode()
   {
      splashScreen.gameObject.SetActive(true);
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

   public void DisableAllContextExceptCredits()
   {
      menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
      menuScreen.SetGameObject(menuScreen.creditsContext, true);
      menuScreen.SetGameObject(menuScreen.optionsContext, false);
      menuScreen.SetGameObject(menuScreen.controlsContext, false);
      menuScreen.SetGameObject(menuScreen.exitContext, false);
   }

   public void ShowCreditsArrow()
   {
      menuScreen.CreditsArrow(true);
   }

   public void HideCreditsArrow()
   {
      menuScreen.CreditsArrow(false);
   }


   private void MainMenuButtons(bool ActiveOrNot)
   {
      menuScreen.MainMenuButtons(ActiveOrNot);
   }

   private void SetCanvasState(CustomCanvas screen, bool boolean)
   {
      screen.SetActiveCanvas(boolean);
   }

   private void StartMenuLoopSong()
   {
      MenuLoopSongRequest?.Invoke();
   }

   // METHODS FOR EVENT CALLS IN ANIMATIONS.
   public void OnLogoAnimationFinished()
   {
      Debug.Log("termino animacion del logo");
      splashScreen.gameObject.SetActive(false);
      currentMenuFlowState = MenuFlowState.TvClip;
      ManageState(currentMenuFlowState);
   }
   public void OnVideoEnd()
   {
      currentMenuFlowState = MenuFlowState.Menu;
      ManageState(currentMenuFlowState);
   }


   private void SetMenuStateToMainMenuAndStartMenuLoopSong()
   {
      StartMenuLoopSong();
      SetMenuStateToMainMenuWithoutSound();
   }

   private void SetMenuStateToMainMenuWithoutSound()
   {
      currentMenuState = MenuState.MainMenu;
      ManageMenuState(currentMenuState);
   }

   private void SetMenuStateToMainMenuWithSound()
   {
      RequestInteractionSound();
      currentMenuState = MenuState.MainMenu;
      ManageMenuState(currentMenuState);
   }

   private void StartDrinkingBottle()
   {
      RequestInteractionSound();
      currentMenuFlowState = MenuFlowState.DrinkingBottle;
      ManageState(currentMenuFlowState);
   }

   private void ShowOptions()
   {
      RequestInteractionSound();
   }

   private void ShowCredits()
   {
      RequestInteractionSound();
      currentMenuState = MenuState.Credits;
      ManageMenuState(currentMenuState);
   }

   private void ReturnToMainMenuFromCredits()
   {
      RequestInteractionSound();
      DisableAllContexts();
      cameraController.FromCreditsToMainMenu();
   }

   private void ShowControls()
   {
      RequestInteractionSound();
      currentMenuState = MenuState.Controls;
      ManageMenuState(currentMenuState);
   }

   public void BeginLoadingGameplay()
   {
      currentMenuFlowState = MenuFlowState.StartGameplay;
      ManageState(currentMenuFlowState);
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

   private void DisableAllMainMenuStuff()
   {

   }

   private void RequestInteractionSound()
   {
      InteractionSoundMenu?.Invoke();
   }

   private void StartFadeIn()
   {
      GameManager.instance.FadeInScreen();
   }

   private void OnHandsControllerDrinkAnimationStarted()
   {
      CameraDrinkingAngle();
   }

   private void OnHandsControllerDrinkAnimationFinished()
   {
      BeginLoadingGameplay();
   }

   private void OnGameplayStart()
   {
      DisableMenuStuff();
   }

   private void DisableMenuStuff()
   {
      HideTitle();
      HideBottleInWorld();
   }


   
}
