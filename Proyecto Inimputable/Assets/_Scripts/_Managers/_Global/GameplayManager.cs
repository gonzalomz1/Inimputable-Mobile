using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public Basement basement;

    public FirstLevel firstLevel;

    public event Action EnablePlayer;
    public event Action DisablePlayer;
    public event Action GameplayLoaded;

    public GameplayState currentGameplayState;

    [SerializeField] PlayerPresenter playerPresenter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
        SubscribeToGameManagerEvents();
    }

    void SubscribeToGameManagerEvents()
    {
        GameManager.instance.GameplayStart += OnGameplayStartCalled;
        GameManager.instance.GameplayPause += OnGameplayPauseCalled;
        GameManager.instance.GameplayResume += OnGameplayResumeCalled;
        GameManager.instance.GameplayExit += OnGameplayExitCalled;
    }

    private void OnGameplayStartCalled()
    {
        ActivatePlayer();
        ActivateFirstLevel();
        CallForFadeOutScreen();
    }

    public void ActivatePlayerExternally()
{
    EnablePlayer?.Invoke();
}

public void DeactivatePlayerExternally()
{
    DisablePlayer?.Invoke();
}

    private void OnGameplayPauseCalled()
    {

    }

    private void OnGameplayResumeCalled()
    {

    }

    private void OnGameplayExitCalled()
    {

    }

    private void ActivatePlayer()
    {
        EnablePlayer?.Invoke();
    }

    private void ActivateFirstLevel()
    {

    }

    private void CallForFadeOutScreen()
    {
        GameplayLoaded.Invoke();
    }

    }
