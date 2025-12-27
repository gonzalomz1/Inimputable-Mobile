using UnityEngine;
using System;

public class EnterDoorCondition : ObjectiveCondition
{
    [SerializeField] private Door doorToEnter;

    protected override void OnConditionInit()
    {
        if (doorToEnter != null)
            doorToEnter.doorOpenedTrigger += OnDoorTrigger;
    }

    protected override void OnConditionCleanup()
    {
        if (doorToEnter != null)
            doorToEnter.doorOpenedTrigger -= OnDoorTrigger;
    }

    private void OnDoorTrigger()
    {
        CheckCondition();
    }

    private void Update()
    {
        // Safety check: only run if initialized and not yet satisfied
        if (!_initialized || IsSatisfied || doorToEnter == null) return;

        // ALSO check every frame if door is open (in case we missed the event or it was already open)
        if (doorToEnter.IsOpen)
        {
            CheckCondition();
        }
    }

    private void CheckCondition()
    {
        if (doorToEnter == null) return;

        // If door is open...
        if (doorToEnter.IsOpen)
        {
            Debug.Log("EnterDoorCondition: Door Open");
            MarkSatisfied();
        }
    }

}