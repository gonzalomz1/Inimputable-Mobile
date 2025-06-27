using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : CustomCanvas
{

    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Credits credits;
    [SerializeField] private LevelLoader levelLoader;


    public void PlayLogoAnimation()
    {
        credits.PlayLogoAnimation();
    }

    public void SplashScreenOnLogoAnimationFinished()
    {
        menuManager.OnLogoAnimationFinished();
    }

    public void LoadGameplay()
    {
        levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
    public void EnableLoading()
    {
        if (levelLoader == null)
        {
            Debug.LogError("No se inicializo la variable loading");
            return;
        }
        levelLoader.gameObject.SetActive(true);
    }
    public void DisableLoading()
    {
        if (levelLoader == null)
        {
            Debug.LogError("No se inicializo la variable loading");
            return;
        }
        levelLoader.gameObject.SetActive(false);
    }



}
