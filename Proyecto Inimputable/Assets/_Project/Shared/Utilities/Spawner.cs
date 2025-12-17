using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }
    
    public GameObject SpawnEnemy(Transform enemySpawnPosition, TurroController.EnemyType type = TurroController.EnemyType.Ranged)
    {
        if (enemySpawnPosition == null) return null;

        GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy", enemySpawnPosition.position, enemySpawnPosition.rotation);
        if (enemy != null)
        {
            var controller = enemy.GetComponent<TurroController>();
            if (controller != null)
            {
                controller.enemyType = type;
                controller.Spawn();
            }
        }
        return enemy;
    }

    public void ClearAllActiveEnemies()
    {
        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.DeactivateAllPooledObjects();
        }
    }

    public void SpawnPistol(Transform spawnPoint)
    {
        if (spawnPoint == null) return;
        
        if (ObjectPooler.Instance != null)
        {
            GameObject pistol = ObjectPooler.Instance.SpawnFromPool("Pistol", spawnPoint.position, spawnPoint.rotation);
            // Additional logic if needed
        }
    }
    public GameObject SpawnItem(string tag, Vector3 position, Quaternion rotation)
    {
        if (ObjectPooler.Instance != null)
        {
            return ObjectPooler.Instance.SpawnFromPool(tag, position, rotation);
        }
        return null;
    }
}