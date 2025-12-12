using UnityEngine;
using System;

public class EnterDoorCondition : ObjectiveCondition
{
    [SerializeField] private Door doorToEnter;

    private void Start()
    {
        if (doorToEnter != null)
            doorToEnter.doorOpenedTrigger += OnDoorTrigger;
    }

    private void OnDoorTrigger()
    {
        Debug.Log("Door triggered. MarkSatisfied()");
        MarkSatisfied();
    }
}