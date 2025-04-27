using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
public Tv tv;
public MenuManager menuManager;
public void OnVideoEnd(){

menuManager.OnVideoEnd();   
    
}



}
