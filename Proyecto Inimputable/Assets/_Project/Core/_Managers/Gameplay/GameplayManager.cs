using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public Basement basement;
    public FirstLevel firstLevel;
    public GameplayState currentGameplayState;

    [SerializeField] PlayerManager playerManager;

    public event Action EnablePlayer;
    public event Action DisablePlayer;

    public event Action TeleportStatePlayer;

    public event Action GameplayLoaded;

    public event Action FirstRoomLock;
    public event Action FirstRoomUnlock;
    public event Action SecondRoomLock;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        SubscribeToGameManagerEvents();
        SubscribeToPlayerTeleporterEvents();
        
        ManageGameplayState(currentGameplayState);
    }

    void SubscribeToGameManagerEvents()
    {
        GameManager.instance.GameplayStart += OnGameplayStartCalled;
        GameManager.instance.GameplayPause += OnGameplayPauseCalled;
        GameManager.instance.GameplayExit += OnGameplayExitCalled;
    }

    void SubscribeToPlayerTeleporterEvents()
    {
        PlayerTeleporter.instance.DisablePlayerForTeleport += OnDisablePlayerForTeleport;
        PlayerTeleporter.instance.EnablePlayerAfterTeleport += OnEnablePlayerForTeleport;
    }

    private void ManageGameplayState(GameplayState state)
    {
        switch (state)
        {
            case GameplayState.Disabled:
            DisablePlayer?.Invoke();
            break;
            case GameplayState.Running:
            EnablePlayer?.Invoke();
            break;
            case GameplayState.TeleportingPlayer:
            TeleportStatePlayer?.Invoke();
            break;
            case GameplayState.GameOver:
            break;
        }
    }


    private void OnGameplayStartCalled()
    {
        ActivatePlayerExternally();
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
        currentGameplayState = GameplayState.Paused;
        ManageGameplayState(currentGameplayState);
    }

    private void OnGameplayExitCalled()
    {
        currentGameplayState = GameplayState.Disabled;
        ManageGameplayState(currentGameplayState);
    }

    private void OnDisablePlayerForTeleport()
    {
        currentGameplayState = GameplayState.TeleportingPlayer;
        ManageGameplayState(currentGameplayState);
    }

    private void OnEnablePlayerForTeleport()
    {
        currentGameplayState = GameplayState.Running;
        ManageGameplayState(currentGameplayState);
    }


    private void CallForFadeOutScreen()
    {
        GameplayLoaded.Invoke();
    }

}
