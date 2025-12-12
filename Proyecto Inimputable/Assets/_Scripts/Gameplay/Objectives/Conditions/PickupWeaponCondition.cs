using UnityEngine;

public class PickupWeaponCondition : ObjectiveCondition
{
    [SerializeField] private WeaponType targetWeaponType;

    protected override void OnConditionInit()
    {
        if (WeaponController.instance != null)
        {
            WeaponController.instance.OnWeaponPickedUp += OnWeaponPickedUp;
            
            // Check if already picked up (retroactive check)
            if (WeaponController.instance.weaponModel != null && 
                WeaponController.instance.weaponModel.pickedUpWeapons.Contains(targetWeaponType))
            {
                MarkSatisfied();
            }
        }
    }

    private void OnWeaponPickedUp(WeaponType type)
    {
        if (!_initialized) return;
        
        if (type == targetWeaponType)
        {
            MarkSatisfied();
        }
    }

    protected override void OnConditionCleanup()
    {
        if (WeaponController.instance != null)
        {
            WeaponController.instance.OnWeaponPickedUp -= OnWeaponPickedUp;
        }
    }
}
