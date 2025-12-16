using UnityEngine;

public class KillCountCondition : ObjectiveCondition
{
    [SerializeField] private int requiredKills;
    private int currentKills;

    protected override void OnConditionInit()
    {
        currentKills = 0;
        // conexion con enemies manager para que envie una señal
    }

    private void OnEnemyKilled()
    {
        if (!_initialized) return;

        currentKills++;
        if (currentKills >= requiredKills)
            MarkSatisfied();
    }

    protected override void OnConditionCleanup()
    {
        // desconectar el enemy manager de esta condition
    }
}