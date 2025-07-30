using System;
using UnityEngine;

public class PlayerPresenter : Player
{
    public PlayerState playerState;

    public event Action TakeDamage;
    public event Action Dead;
    public event Action Alive;


    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private PlayerData playerData; // model
    [SerializeField] private WeaponController weaponController; // view
    [SerializeField] private MovementGameObjectMessager movementGameObjectMessager;



    void Start()
    {
        SubscribeToGameManagerEvents();
        SubscribeToPlayerEvents();
        ManagePlayerState(playerState);
    }

    private void SubscribeToGameManagerEvents()
    {
        gameplayManager.EnablePlayer += OnEnablePlayer;
        gameplayManager.DisablePlayer += OnDisablePlayer;
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
        //
    }

    public void PlayerDead()
    {
        Dead.Invoke();
    }

}
