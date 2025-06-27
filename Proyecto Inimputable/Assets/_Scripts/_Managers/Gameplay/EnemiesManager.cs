using UnityEngine;
using System.Collections.Generic;

public class EnemiesManager : MonoBehaviour
{
    public ObjectiveManager objectiveManager;
    [SerializeField] private bool needObjetive = true;
    [SerializeField] private int totalEnemiesToSpawn = 5;
    [SerializeField] private Transform[] spawnPoints;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public void ActivateEnemies()
    {
        objectiveManager.totalEnemies = totalEnemiesToSpawn;
        objectiveManager.remainingEnemies = totalEnemiesToSpawn;

        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject enemy = ObjectPooler.Instance.SpawnFromPool("Enemy", spawnPoint.position, spawnPoint.rotation);

            if (enemy != null)
            {
                TurroController turro = enemy.GetComponent<TurroController>();
                if (turro != null)
                {
                    turro.SetState(EnemyState.Spawn);
                    turro.turroState = EnemyState.Spawn;
                }
                activeEnemies.Add(enemy);
            }
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        if (needObjetive)
        {
            objectiveManager.CheckObjectiveCondition();
        }

        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            ObjectPooler.Instance.ReturnToPool("Enemy", enemy);
        }
    }

    public void DeactivateAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            ObjectPooler.Instance.ReturnToPool("Enemy", enemy);
        }
        activeEnemies.Clear();
    }
}