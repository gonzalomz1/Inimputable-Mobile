using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
        public void ActivateEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");

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
}
