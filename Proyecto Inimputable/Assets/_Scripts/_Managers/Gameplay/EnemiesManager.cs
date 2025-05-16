using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private int totalEnemies;
    private int remainingEnemies;


    public void ActivateEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
        totalEnemies = enemies.Length;
        remainingEnemies = totalEnemies;

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
        remainingEnemies--;

        Debug.Log($"Un enemigo murió. Quedan {remainingEnemies}");

        if (remainingEnemies <= 0)
        {
            GetComponentInParent<GameFlowManager>().GameOver();
        }
    }
}
