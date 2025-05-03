using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : CustomCanvas

{
    public Button play;
    public Button options;
    public Button credits;
    public Button exit;

    public MenuManager menuManager;

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }

   public void SetTextMeshTransform(List<Vector2>list ){
        play.transform.position = list[0];
        options.transform.position = list[1];
        credits.transform.position = list[2];
        exit.transform.position = list[3];
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


   
}
