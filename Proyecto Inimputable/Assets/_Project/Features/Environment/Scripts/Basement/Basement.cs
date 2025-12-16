using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    public Tv tv;
    public MenuManager menuManager;

    public GameObject bottle;


    public void OnVideoEnd()
    {

        menuManager.OnVideoEnd();

    }

    public void HideBottle()
    {
        bottle.SetActive(false);
    }

    public void ShowBottle()
    {
        bottle.SetActive(true);
    }
}
