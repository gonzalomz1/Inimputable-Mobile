using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        SubscribeToGameManagerEvents();
    }

    void SubscribeToGameManagerEvents()
    {
        gameManager.GameExecute += OnGameExecute;
        gameManager.FadeIn += OnFadeIn;
        gameManager.FadeOut += OnFadeOut;
    }

    void OnGameExecute()
    {
        DisableScreenFader();
    }

    void OnFadeIn()
    {
        screenFader.FadeIn();
    }

    void OnFadeOut()
    {
        screenFader.FadeOut();
    }

    private void DisableScreenFader()
    {
        print("disabling screen fader");
        screenFader.gameObject.SetActive(false);
    }

    private void EnableScreenFader()
    {
        screenFader.gameObject.SetActive(true);
    }
}
