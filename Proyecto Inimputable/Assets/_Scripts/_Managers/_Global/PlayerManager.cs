using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [SerializeField] private PlayerPresenter playerPresenter;

    public event Action DisablePlayer;
    public event Action EnablePlayer;
    public event Action TeleportStatePlayer;

    public event Action<Vector2> SendInputDataToPlayerPresenter;

    void Awake()
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
        SubscribeToGameplayManagerEvents();
        SubscribeToMoveAndAimCanvasEvents();
    }

    private void SubscribeToGameManagerEvents()
    {
        gameManager.GameExecute += OnGameExecute;
        gameManager.GameplayStart += OnGameplayStart;
    }

    private void SubscribeToGameplayManagerEvents()
    {
        GameplayManager.instance.EnablePlayer += EnablePlayerGameObject;
        GameplayManager.instance.DisablePlayer += DisablePlayerGameObject;
        GameplayManager.instance.TeleportStatePlayer += OnTeleportStatePlayer;
    }

    private void SubscribeToMoveAndAimCanvasEvents()
    {
        MovAndAimCanvas.Instance.InputDataRequest += ProcessInputDataRequest;
    }


    private void OnGameExecute()
    {
        DisablePlayerGameObject();
    }

    private void OnGameplayStart()
    {
        EnablePlayerGameObject();
    }

    private void DisablePlayerGameObject()
    {
        DisablePlayer?.Invoke();
    }

    private void EnablePlayerGameObject()
    {
        EnablePlayer?.Invoke();
    }

    private void OnTeleportStatePlayer()
    {
        
    }

    private void ProcessInputDataRequest(Vector2 amount)
    {
        SendMovementInputDataToPlayer(amount);
    }

    public void SendMovementInputDataToPlayer(Vector2 amount)
    {
        SendInputDataToPlayerPresenter?.Invoke(amount);
    }

    public bool CanPlayerProcessInput()
    {
        if (playerPresenter.IsPlayerActive()) return true;
        else return false;
    }


}
