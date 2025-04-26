using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
 public Credits credits;

 public Loading loading;

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
    if (loading == null){
        Debug.LogError("No se inicializo la variable loading");
        return;
    }
    loading.gameObject.SetActive(true);
 }
public void DisableLoading() {
    if (loading == null){
        Debug.LogError("No se inicializo la variable loading");
        return;
    }
    loading.gameObject.SetActive(false);
 }

}
