using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public enum GameState { Credits, PlayingClip, Menu, Gameplay }

public enum MenuState{ MainMenu, Credits, Options, Exit, Play}
public class MenuManager : MonoBehaviour
{
   public SplashScreen splashScreen;

   public GameState currentState;

   public MenuState currentMenuState;

   public Basement basement;
   
   public MenuCamera menuCamera;




   void Start()
   {
      currentState = GameState.Credits;
      ManageState(currentState);

      currentMenuState = MenuState.MainMenu;

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
   public void OnCreditsAnimationFinished()
   {
      Debug.Log("termino animacion");

      splashScreen.gameObject.SetActive(false);

      currentState = GameState.PlayingClip;
      ManageState(currentState);


   }
    public void OnVideoEnd(){

      currentState = GameState.Menu;
      ManageState(currentState);
   

    
}
}
