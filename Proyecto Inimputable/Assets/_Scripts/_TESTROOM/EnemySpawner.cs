using UnityEngine;

public class EnemySpawner : MonoBehaviour, IInteractive
{
    [SerializeField] private GameObject enemyRange;
    [SerializeField] private Transform spawnPoint;

    public void OnInteraction()
    {
        GameObject newEnemy = Instantiate(enemyRange, spawnPoint.position, spawnPoint.rotation);
        newEnemy.GetComponent<TurroController>().SetState(EnemyState.Spawn);
    }

}
