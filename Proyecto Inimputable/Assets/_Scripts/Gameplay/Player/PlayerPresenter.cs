using UnityEngine;
using System;


public class PlayerPresenter : Player
{
    public PlayerState playerState;

    public event Action TakeDamage;
    public event Action Dead;
    public event Action Alive;

    public event Action<Vector2> SendInputData;


    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private WeaponController weaponController; // view
    [SerializeField] private MovementGameObjectMessager movementGameObjectMessager;



    void Start()
    {
        SubscribeToGameManagerEvents();
        SubscribeToPlayerManagerEvents();
        SubscribeToPlayerEvents();
        ManagePlayerState(playerState);
    }

    private void SubscribeToGameManagerEvents()
    {
        gameplayManager.EnablePlayer += OnEnablePlayer;
        gameplayManager.DisablePlayer += OnDisablePlayer;
    }

    private void SubscribeToPlayerManagerEvents()
    {
        PlayerManager.instance.EnablePlayer += OnEnablePlayer;
        PlayerManager.instance.DisablePlayer += OnDisablePlayer;
        PlayerManager.instance.SendInputDataToPlayerPresenter += SendInputDataToPlayerData;
    }

    private void SubscribeToPlayerEvents()
    {
        movementGameObjectMessager.MovementSoundRequest += OnMovementSoundRequest;
    }
    


    private void OnEnablePlayer()
    {
        playerState = PlayerState.StartFromGameplay;
        ManagePlayerState(playerState);
    }

    private void OnDisablePlayer()
    {
        playerState = PlayerState.Disabled;
        ManagePlayerState(playerState);
    }   

    private void OnGameplayExit()
    {
        playerState = PlayerState.Disabled;
        ManagePlayerState(playerState);
    }

    private void ManagePlayerState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Disabled:
                PlayerDisabledMode();
                break;
            case PlayerState.StartFromGameplay:
                PlayerEnabledMode();
                break;
        }
    }

    private void OnMovementSoundRequest()
    {
        GameManager.instance.PlayerMovementSound();
    }



    private void SetupToStartGameplay()
    {
        // for now nothing.
    }

    private void PlayerDisabledMode()
    {
        playerData.SetPlayerPhysicalState(false);
        weaponController.enabled = false;
    }

    private void PlayerEnabledMode()
    {
        playerData.SetPlayerPhysicalState(true);
        weaponController.enabled = true;
    }

    public void PlayerDead()
    {
        Dead.Invoke();
    }

    public void SendInputDataToPlayerData(Vector2 amount)
    {
        SendInputData(amount);
    }

    public bool IsPlayerActive()
    {
        if (playerState == PlayerState.Disabled) return false;
        else return true;
    }

}
