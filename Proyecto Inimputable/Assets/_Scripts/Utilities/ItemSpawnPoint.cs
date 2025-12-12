using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    [Header("Pool Config")]
    [Tooltip("Tag matched in ObjectPooler")]
    public string itemTag = "Items";
    
    [Header("Item Config")]
    public PickupItem.PickupType itemType;

    void OnEnable()
    {
        // Spawning on Scene Start
        GameManager.instance.GameplayStart += SpawnLogic;
        // Spawning on Retry
        GameManager.instance.GameRetry += SpawnLogic;
    }

    void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.GameplayStart -= SpawnLogic;
            GameManager.instance.GameRetry -= SpawnLogic;
        }
    }

    private void SpawnLogic()
    {
        if (Spawner.instance != null)
        {
            GameObject obj = Spawner.instance.SpawnItem(itemTag, transform.position, transform.rotation);
            if (obj != null)
            {
                var pickup = obj.GetComponent<PickupItem>();
                if (pickup != null)
                {
                    pickup.Configure(itemType);
                }
            }
        }
    }
}
