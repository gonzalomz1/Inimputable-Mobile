using UnityEngine;

public class KillThiefObjective : ObjectiveBase
{
    private TurroController _targetEnemy;

    public void SetTarget(GameObject target)
    {
        _targetEnemy = target.GetComponent<TurroController>();
        if (_targetEnemy != null)
        {
            _targetEnemy.Dead += OnTargetDead;
        }
    }

    public override void Tick()
    {
        // Optional: Fail safe check if object is destroyed without event
        if (_targetEnemy == null && !IsCompleted) 
        {
           // CheckForCompletion(); // Or handle error
        }
    }

    private void OnTargetDead()
    {
        if (_targetEnemy != null)
            _targetEnemy.Dead -= OnTargetDead;
            
        Complete();
    }
}