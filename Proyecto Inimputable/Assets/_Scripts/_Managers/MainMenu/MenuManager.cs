using System;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { Credits, PlayingClip, Menu, Gameplay }
public enum MenuState { MainMenu, Credits, Options, Exit, Play }

public class MenuManager : MonoBehaviour
{
   public SplashScreen splashScreen;

   public GameState currentState;

   public MenuState currentMenuState;

   public Basement basement;

   public MenuCamera menuCamera;

   public List<TextPosition> textPositions;

   public MenuScreen menuScreen;

   


   void Start()
   {
      currentState = GameState.Credits;
      currentMenuState = MenuState.MainMenu;
      ManageState(currentState);
      ManageMenuState(currentMenuState);
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
   void ManageMenuState(MenuState current)
   {
      if (currentState == GameState.Menu)
      {
         switch (current)

         {
            case MenuState.MainMenu:
               /*acomodar los textos */
               //habiliar el touch 

               SetCanvasState(menuScreen, true);

               break;

         }

      }
      else
      {
         SetCanvasState(menuScreen, false);
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
   public void SetMainMenuTexts(){
      List<Vector2>positions= new List<Vector2>();
    positions.Add(SelectTextPosition(0));// posicion de play
    positions.Add(SelectTextPosition(1));// posicion de option
    positions.Add(SelectTextPosition(2));// posicion de credit
    positions.Add(SelectTextPosition(3));// posicion de exit
   
   menuScreen.SetTextMeshTransform(positions);
   
   }
}
