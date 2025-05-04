using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : CustomCanvas

{
    public Button mainMenuPlay;
    public Button mainMenuOptions;
    public Button mainMenuCredits;
    public Button mainMenuExit;

    public Button creditsExit;

    public Button optionsBrightness; 
    
     public Button optionsExit;

     public Button exitExit;

    public MenuManager menuManager;
    public GameObject credits;

    public GameObject mainMenu;

    public GameObject options;

    public GameObject exit;
    

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }

   public void SetTextMeshTransform(List<Vector2>list ){
        mainMenuPlay.transform.position = list[0];
        mainMenuOptions.transform.position = list[1];
        mainMenuCredits.transform.position = list[2];
        mainMenuExit.transform.position = list[3];
        menuManager.AfterTextPositionSet();
   }

   public void Play(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex
     + 1);
   }

   public void Options(){

   }

   public void Credits(){

   }

   public void Exit(){

   }

public void SetGameObject(GameObject gameObject, bool boolean){
    gameObject.SetActive(boolean);
  
}
   
}
