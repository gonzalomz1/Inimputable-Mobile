using UnityEngine;

public class PickupItemCondition : ObjectiveCondition
{
    [SerializeField] private ItemBase item;

    protected override void OnConditionInit()
    {
        item.OnPickup += OnItemPicked;
    }


    private void OnItemPicked()
    {
        if (!_initialized) return;
        MarkSatisfied();
    }

    protected override void OnConditionCleanup()
    {
        item.OnPickup -= OnItemPicked;
    }

}