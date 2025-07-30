using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    private List<GameObject> objectiveEnemies = new List<GameObject>();

    public int totalEnemies => objectiveEnemies.Count;
    public int remainingEnemies;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }

    public void RegisterObjectiveEnemy(GameObject enemy)
    {
        if (!objectiveEnemies.Contains(enemy))
        {
            objectiveEnemies.Add(enemy);
            remainingEnemies = objectiveEnemies.Count;
        }
    }

    // Llamar cuando un enemigo muere para descontarlo
    public void EnemyKilled(GameObject enemy)
    {
        if (objectiveEnemies.Contains(enemy))
        {
            objectiveEnemies.Remove(enemy);
            remainingEnemies = objectiveEnemies.Count;
            Debug.Log($"Enemigo eliminado. Quedan {remainingEnemies}");

            if (remainingEnemies <= 0)
            {
                OnObjectiveCompleted();
            }
        }
    }

    private void OnObjectiveCompleted()
    {
        GameManager.instance.SetGameOver();
    }

    public void ResetObjective()
    {
        objectiveEnemies.Clear();
        remainingEnemies = 0;
    }
}