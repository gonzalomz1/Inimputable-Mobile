using UnityEngine;

public class Door : MonoBehaviour, IInteractive
{
    [SerializeField] private DoorState doorState;
    [SerializeField] private Transform playerSpawnPosition;   // Lugar donde aparecerá el jugador al usar esta puerta
    [SerializeField] private Transform enemySpawnPosition;    // Por si más adelante quieres spawnear enemigos
    [SerializeField] private Animator animator;
    [SerializeField] private bool isLocked = false;

    [Header("Linked Door")]
    [Tooltip("Puerta a la que se teletransportará el jugador")]
    [SerializeField] private Door linkedDoor;

    private bool isOpen = false;

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

    private void SetState(DoorState state)
    {
        switch (state)
        {
            case DoorState.Default:
                animator?.SetTrigger("Default");
                isOpen = false;
                break;
            case DoorState.Open:
                animator?.SetTrigger("Open");
                isOpen = true;
                break;
            case DoorState.Locked:
                animator?.SetTrigger("Locked");
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

public GameObject SpawnEnemy()
{
    if (enemySpawnPosition == null) return null;

    GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy", enemySpawnPosition.position, enemySpawnPosition.rotation);
    if (enemy != null)
    {
        var controller = enemy.GetComponent<TurroController>();
        if (controller != null)
        {
            controller.SetState(EnemyState.Spawn);
        }
    }
    return enemy;
}
}
