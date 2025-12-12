using UnityEngine;
using System;

public class Door : MonoBehaviour, IInteractive
{
    [SerializeField] private DoorState doorState;
    [SerializeField] private Transform playerSpawnPosition;   
    [SerializeField] private Transform enemySpawnPosition;    
    [SerializeField] private Animator animator;
    [SerializeField] private bool isLocked = false;

    [Header("Linked Door")]
    [Tooltip("Puerta a la que se teletransportará el jugador")]
    [SerializeField] private Door linkedDoor;

    private bool isOpen = false;

    public event Action doorOpenedTrigger;

    private void Start()
    {
        SetState(doorState);
    }

    public void OnInteraction()
    {
        TryToOpenDoor(out _);
    }

    /// <summary>
    /// Intenta abrir la puerta. Devuelve true y la puerta de destino si el jugador puede cruzar.
    /// </summary>
    public bool TryToOpenDoor(out Door targetDoor)
    {
        targetDoor = null;

        if (isLocked)
        {
            SetState(DoorState.Locked);
            return false;
        }

        SetState(DoorState.Open);

        // Si hay una puerta vinculada, la devolvemos para teletransportar
        if (linkedDoor != null)
        {
            targetDoor = linkedDoor;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Devuelve la posición donde debe colocarse el jugador al llegar por esta puerta.
    /// </summary>
    public Vector3 GetPlayerSpawnPosition()
    {
        return playerSpawnPosition.position;
    }

    public Quaternion GetSpawnRotation()
    {
        return playerSpawnPosition.rotation;
    }

    public void DefaultStateDoor() => SetState(DoorState.Default);
    public void LockStateDoor() => SetState(DoorState.Locked);

    private void SetState(DoorState state)
    {
        Debug.Log($"Door {gameObject.name}: Switching state to {state}");
        doorState = state;
        switch (state)
        {
            case DoorState.Default:
                animator?.SetTrigger("Default");
                isOpen = false;
                isLocked = false; // Unlock logically
                break;
            case DoorState.Open:
                animator?.SetTrigger("Open");
                isOpen = true;
                // isLocked state doesn't necessarily change here, but usually implies unlocked
                doorOpenedTrigger?.Invoke();
                break;
            case DoorState.Locked:
                animator?.SetTrigger("Locked");
                isLocked = true; // Lock logically
                break;
            case DoorState.Close:
                animator?.SetTrigger("Close");
                isOpen = false;
                break;
            case DoorState.Disabled:
                gameObject.SetActive(false);
                break;
        }
    }

    public Transform GetEnemiesSpawnPosition()
    {
        return enemySpawnPosition;
    }




}
