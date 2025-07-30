using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [SerializeField] private PlayerPresenter playerPresenter;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
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

    public void GetPlayerPosition()
    {
        //Transform position = playerPresenter.GetPlayerPosition();
    }

    public void GetPlayerRotation()
    {
        //
    }


}
