using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : CustomCanvas
{
 public Credits credits;

 public LevelLoader levelLoader;

 public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }

public void EnableCredits (){
    if (credits == null){
        Debug.LogError("No se inicializo la variable credits");
        return;
    }
    credits.gameObject.SetActive(true);
 }
 public void DisableCredits() {
    if (credits == null){
        Debug.LogError("No se inicializo la variable credits");
        return;
    }
    credits.gameObject.SetActive(false);
 }
public void EnableLoading (){
    if (levelLoader == null){
        Debug.LogError("No se inicializo la variable loading");
        return;
    }
    levelLoader.gameObject.SetActive(true);
 }
public void DisableLoading() {
    if (levelLoader == null){
        Debug.LogError("No se inicializo la variable loading");
        return;
    }
    levelLoader.gameObject.SetActive(false);
 }

 public void LoadGameplay()
 {
    levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex+ 1);
 }

}
