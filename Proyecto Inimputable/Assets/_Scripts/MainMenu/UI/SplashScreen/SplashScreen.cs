using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : CustomCanvas
{

    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Credits credits;

    public void PlayLogoAnimation()
    {
        credits.PlayLogoAnimation();
    }

    public void SplashScreenOnLogoAnimationFinished()
    {
        menuManager.OnLogoAnimationFinished();
    }


    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }

    public void EnableCredits()
    {
        if (credits == null)
        {
            Debug.LogError("No se inicializo la variable credits");
            return;
        }
        credits.gameObject.SetActive(true);
    }
    public void DisableCredits()
    {
        if (credits == null)
        {
            Debug.LogError("No se inicializo la variable credits");
            return;
        }
        credits.gameObject.SetActive(false);
    }



}
