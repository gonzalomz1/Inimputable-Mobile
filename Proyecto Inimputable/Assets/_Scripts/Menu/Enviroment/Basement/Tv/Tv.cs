using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tv : MonoBehaviour
{

 public TvScreen tvScreen;
public Basement basement;
public void OnVideoEnd(){

basement.OnVideoEnd();
    
}


}
