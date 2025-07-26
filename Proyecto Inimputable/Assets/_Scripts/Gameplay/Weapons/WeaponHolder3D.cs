using UnityEngine;

enum WeaponHolderState { Active, PickedUp, Destroyed }
public class WeaponHolder3D : MonoBehaviour
{
    [SerializeField] private bool needToMakeAction = true;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponHolderState currentState;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody rigidBody;

    private bool hasSpawnedEnemies = false; // Para evitar doble spawn
    private FirstLevel firstLevel;

    void Awake()
    {
        if (!weaponData) return;
        if (!spriteRenderer) GetComponent<SpriteRenderer>();
        if (!sphereCollider) GetComponent<SphereCollider>();
        if (!rigidBody) GetComponent<Rigidbody>();
        firstLevel = FindObjectOfType<FirstLevel>();
    }

    void Start()
    {
        currentState = WeaponHolderState.Active;
        ManageState(currentState);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = WeaponHolderState.PickedUp;
            ManageState(currentState);

            if (!hasSpawnedEnemies)
            {
                SpawnEnemiesFromDoors();
                hasSpawnedEnemies = true;
            }
        }
    }

    void SpawnEnemiesFromDoors()
    {
        if (firstLevel == null)
        {
            Debug.LogWarning("No se encontró FirstLevel en la escena.");
            return;
        }

        Door[] spawnDoors = firstLevel.GetEnemySpawnDoors();
        foreach (Door door in spawnDoors)
        {
            door.SpawnEnemy();  // Usa el ObjectPooler internamente
        }
    }

    void ManageState(WeaponHolderState current)
    {
        switch (current)
        {
            case WeaponHolderState.Active:
                SetSprite(weaponData);
                break;
            case WeaponHolderState.PickedUp:
                SetAsPickedUpOnPlayer(weaponData);
                currentState = WeaponHolderState.Destroyed;
                ManageState(currentState);
                break;
            case WeaponHolderState.Destroyed:
                Destroy(this.gameObject);
                break;
        }
    }

    void SetSprite(WeaponData wd)
    {
        if (wd != null && spriteRenderer != null)
            spriteRenderer.sprite = wd.weaponIcon;
    }

    private void SetAsPickedUpOnPlayer(WeaponData wd)
    {
        GameObject player = GameObject.FindWithTag("Player");
        WeaponController weaponController = player.GetComponent<WeaponController>();
        switch (wd.weaponName)
        {
            case "Pistol":
                weaponController.PickUpWeapon(WeaponType.Pistol);
                break;
        }
    }
}
