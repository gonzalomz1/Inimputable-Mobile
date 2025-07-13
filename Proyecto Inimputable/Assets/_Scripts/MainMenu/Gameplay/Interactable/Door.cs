using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractive
{
    [SerializeField] private DoorState doorState;
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private Transform enemySpawnPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isLocked = false;

    void Start()
    {
        SetState(doorState);
    }

    public void OnInteraction()
    {
        TryToOpenDoor();
    }

    private void TryToOpenDoor()
    {
        if (isLocked)
        {
            SetState(DoorState.Locked);
            return;
        }
        SetState(DoorState.Open);
    }

    private void SetState(DoorState state)
    {
        switch (state)
        {
            case DoorState.Default:
                animator.SetTrigger("Default");
                break;
            case DoorState.Open:
                animator.SetTrigger("Open");
                break;
            case DoorState.Locked:
                animator.SetTrigger("Locked");
                break;
            case DoorState.Close:
                animator.SetTrigger("Close");
                break;
            case DoorState.Disabled:
                gameObject.SetActive(false);
                break;
        }
    }

    public void SpawnEnemy(){

    }

}
