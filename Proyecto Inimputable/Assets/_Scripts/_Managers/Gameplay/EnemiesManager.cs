using UnityEngine;
using System.Collections.Generic;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance;

    
    [SerializeField] private bool needObjetive = true;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        if (needObjetive)
        {

        }

        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            ObjectPooler.Instance.ReturnToPool("Enemy", enemy);
        }
    }

    public void ClearEnemies()
    {
        foreach (var enemy in activeEnemies)
        {
            ObjectPooler.Instance.ReturnToPool("Enemy", enemy);
        }
        activeEnemies.Clear();
    }

    public GameObject[] GetActiveEnemiesArray()
{
    return activeEnemies.ToArray();
}
}