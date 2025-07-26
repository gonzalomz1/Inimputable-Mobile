using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    private List<GameObject> objectiveEnemies = new List<GameObject>();

    public int totalEnemies => objectiveEnemies.Count;
    public int remainingEnemies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Registrar un enemigo que forma parte del objetivo (llamalo cuando spawneas)
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
        Debug.Log("¡Objetivo completado! Todos los enemigos eliminados.");
        // Aquí poné la lógica para el GameOver o siguiente nivel
        // Ejemplo: GameManager.instance.GameOver();
    }

    // Opcional: resetear la lista cuando inicies nueva ronda o escena
    public void ResetObjective()
    {
        objectiveEnemies.Clear();
        remainingEnemies = 0;
    }
}