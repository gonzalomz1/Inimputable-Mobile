using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;





public class MenuManager : MonoBehaviour
{
   [Header("State")]
   public GameState currentState;
   public MenuState currentMenuState;
   [Header("Components")]
   public SplashScreen splashScreen;
   public Basement basement;
   public MenuCamera menuCamera;
   public MenuScreen menuScreen;
   [Header("Position for Main Menu buttons")]
   public List<TextPosition> textPositions;
   [Header("Raycaster")]
   public GraphicRaycaster raycaster;
   [Header("Event Object")]
   public EventSystem eventSystem;
   private Finger menuFinger;


   void Start()
   {
      SetCameraDefaultPosition();
      currentState = GameState.Credits;
      currentMenuState = MenuState.Disable;
      ManageState(currentState);
      ManageMenuState(currentMenuState);
   }
      void SetCameraDefaultPosition() {
      menuCamera.transform.position = new Vector3(-0.38999998569488528f, 0.6000000238418579f,0.05999999865889549f);
      menuCamera.transform.rotation = new Quaternion(0.0f,-0.7071068286895752f,0.0f,0.7071068286895752f);
      }

   void ManageState(GameState current)
   {
      switch (current)
      {
         case GameState.Credits:
            splashScreen.gameObject.SetActive(true);
            splashScreen.DisableLoading();
            splashScreen.EnableCredits();
            break;
         case GameState.PlayingClip:
            basement.tv.tvScreen.PlayVideo();
            break;
         case GameState.Menu:
            menuCamera.MainMenuAngle();
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
            EnableInput();
            SetCanvasState(menuScreen, true);
            menuScreen.SetGameObject(menuScreen.creditsContext, false);
            menuScreen.SetGameObject(menuScreen.optionsContext, false);
            menuScreen.SetGameObject(menuScreen.mainMenuContext, true);
            menuScreen.SetGameObject(menuScreen.exitContext, false);
            break;
         case MenuState.Credits:
            menuCamera.CreditsAngle();
            menuScreen.SetGameObject(menuScreen.creditsContext, true);
            menuScreen.CreditsArrow(false);
            menuScreen.SetGameObject(menuScreen.mainMenuContext,false);
            menuScreen.SetGameObject(menuScreen.optionsContext, false);
            menuScreen.MainMenuButtons(false);
           break;
           case MenuState.Options:
            menuScreen.SetGameObject(menuScreen.optionsContext, true);
            menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
           break;
           
          case MenuState.Exit:
            menuScreen.SetGameObject(menuScreen.exitContext, true);
            menuScreen.SetGameObject(menuScreen.mainMenuContext, false);
            menuScreen.SetGameObject(menuScreen.optionsContext, false);
            menuScreen.SetGameObject(menuScreen.creditsContext, false);
           break;

      }

   }

   private void SetCanvasState(CustomCanvas screen, bool boolean)
   {
      screen.SetActiveCanvas(boolean);
   }
   public void OnCreditsAnimationFinished()
   {
      Debug.Log("termino animacion del logo");
      splashScreen.gameObject.SetActive(false);
      currentState = GameState.PlayingClip;
      ManageState(currentState);
   }
   public void OnVideoEnd()
   {
      currentState = GameState.Menu;

      ManageState(currentState);
   }
   public Vector2 SelectTextPosition(int listPosition)
   {
      Vector2 pos;
      if (listPosition == 0)
      {
         pos = textPositions[0].TransformPositionToCanvasPosition();
         return pos;
      }
      if (listPosition == 1)
      {
         pos = textPositions[1].TransformPositionToCanvasPosition();
         return pos;
      }
      if (listPosition == 2)
      {
         pos = textPositions[2].TransformPositionToCanvasPosition();
         return pos;
      }
      if (listPosition == 3)
      {
         pos = textPositions[3].TransformPositionToCanvasPosition();
         return pos;
      }
      return Vector2.zero;

   }
   public void SetMainMenuTexts()
   {
      List<Vector2> positions = new List<Vector2>();
      Vector2 p1 = SelectTextPosition(0);
      Vector2 p2 = SelectTextPosition(1);
      Vector2 p3 = SelectTextPosition(2);
      Vector2 p4 = SelectTextPosition(3);
      positions.Add(p1);// posicion de play
      positions.Add(p2);// posicion de option
      positions.Add(p3);// posicion de credit
      positions.Add(p4);// posicion de exit
      menuScreen.SetTextMeshTransform(positions);
   }

   public void AfterTextPositionSet()
   {
      currentMenuState = MenuState.MainMenu;
      ManageMenuState(currentMenuState);
   }

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
      Vector2 touchPos = finger.screenPosition;
      PointerEventData pointerEventData = new PointerEventData(eventSystem);
      pointerEventData.position = touchPos;

      var results = new System.Collections.Generic.List<RaycastResult>();
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

   public void BeginLoadingGameplay()
   {
      splashScreen.SetActiveCanvas(true);
      splashScreen.DisableCredits();
      splashScreen.EnableLoading();
      splashScreen.LoadGameplay();
   }
}
