using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public ObjectiveManager objectiveManager;

    [SerializeField] private bool needObjetive = true;

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
        if (needObjetive) objectiveManager.CheckObjectiveCondition();
        else return;
    }
}
