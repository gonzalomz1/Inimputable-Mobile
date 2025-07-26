using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [SerializeField] private PlayerPresenter playerPresenter;

    void Awake()
    {
        SubscribeToGameManagerEvents();
    }

    private void SubscribeToGameManagerEvents()
    {
        gameManager.GameExecute += OnGameExecute;
        gameManager.GameplayStart += OnGameplayStart;
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
        playerPresenter.gameObject.SetActive(false);
    }

    private void EnablePlayerGameObject()
    {
        playerPresenter.gameObject.SetActive(true);
    }


}
