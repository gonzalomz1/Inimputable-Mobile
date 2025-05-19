using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public ObjectiveManager objectiveManager;



    public void ActivateEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
        objectiveManager.totalEnemies = enemies.Length;
        objectiveManager.remainingEnemies = objectiveManager.totalEnemies;

        foreach (GameObject enemy in enemies)
        {
            TurroController turro = enemy.GetComponent<TurroController>();
            if (turro != null)
            {
                turro.SetState(EnemyState.Spawn);
                turro.turroState = EnemyState.Spawn;
            }
        }
    }

    public void EnemyDied()
    {
        objectiveManager.CheckObjectiveCondition();
    }
}
