using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public GameFlowManager gameFlowManager;
    public EnemiesManager enemiesManager;

    ObjectiveType currentObjective;

    public int totalEnemies;
    public int remainingEnemies;

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