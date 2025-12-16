using Unity.VisualScripting;
using UnityEngine;

public class FirstLevel : MonoBehaviour
{
    [SerializeField] private GameplayManager gameplayManager;

    [Header("Rooms")]
    [SerializeField] private GameObject firstRoom;
    [SerializeField] private GameObject secondRoom;


    void Awake()
    {
        GameManager.instance.GameExecute += OnGameExecute;
        GameManager.instance.GameplayStart += OnGameplayStart;
    }
    
    private void OnGameExecute()
    {
        firstRoom.SetActive(false);
        secondRoom.SetActive(false);
    }
    
    private void OnGameplayStart()
    {
        firstRoom.SetActive(true);
        secondRoom.SetActive(true);
    }
}