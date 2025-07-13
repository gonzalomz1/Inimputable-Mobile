using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

        ObjectiveManager Instance { get; set; }

    public GameFlowManager gameFlowManager;
    public EnemiesManager enemiesManager;

    ObjectiveType currentObjective;

    public int totalEnemies;
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


    public void SetRoomObjective()
    {
        currentObjective = ObjectiveType.KillAll;
    }

    public void CheckObjectiveCondition()
    {
        remainingEnemies--;

        Debug.Log($"Un enemigo murió. Quedan {remainingEnemies}");

        if (remainingEnemies <= 0)
        {
            SetGameOver();
        }
    }

    public void SetGameOver()
    {
        gameFlowManager.GameOver();
    }

}