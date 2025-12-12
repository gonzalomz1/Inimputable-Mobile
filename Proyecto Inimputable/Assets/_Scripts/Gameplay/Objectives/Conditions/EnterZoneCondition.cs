using UnityEngine;

public class EnterZoneCondition : ObjectiveCondition
{
    [SerializeField] private Collider triggerZone;
    
    protected override void OnConditionInit()
    {
        triggerZone.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_initialized) return;
        if (other.CompareTag("Player"))
        {
            MarkSatisfied();
        }
    }

    protected override void OnConditionCleanup()
    {
        triggerZone.enabled = false;
    }

}
