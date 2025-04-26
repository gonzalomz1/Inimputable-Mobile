using System;
using System.Threading.Tasks;
using UnityEngine;

/*Mostrar el canvas de Splash Screen.
Desabilitar LoadingScreen (gameobject)
habilitar Credits (gameobject)
Darle inicio a la animacion de credito.
Al terminar la animacion de credito:
Escondemos el canvas de Splash Screen
Desabilitamos Credits
Comenzamos el clip del inimputable
*/
public enum GameState { Credits, PlayingClip, Menu, Gameplay} 
public class MenuManager : MonoBehaviour
{
public SplashScreen splashScreen;

public GameState currentState;

void Start (){
   currentState = GameState.Credits;
   ManageState (currentState);
}
void ManageState (GameState current){
   switch (current){
      case GameState.Credits:
       
      
   splashScreen.gameObject.SetActive(true);
   splashScreen.DisableLoading(); 
   
   splashScreen.EnableCredits();

      
      
   
      // al terminar la animacion de creditos desabilitar todo lo que tenga que ver con splash screen
      // cambiar el estado del juego a playinclip 
         break;
   }
}

}
