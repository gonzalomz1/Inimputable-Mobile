using UnityEngine;

public class MeleeWeaponObject : WeaponBehaviour
{
    [SerializeField] private Animator animator;
    private bool isSwinging;
    private LayerMask raycastLayers;


    public override void Initialize(WeaponDataSO data, Transform firePoint)
    {
        this.weaponDataSO = data;
        this.firePoint = firePoint;
    }

    public override bool TriggerPull()
    {
        if (isSwinging) return false;
        Swing();
        return true;
    }

    public override void TriggerRelease() { /* useless here */ }
    public override void Reload() { /* useless here */ }

    private void Swing()
    {
        isSwinging = true;
        animator.SetTrigger("Swing");
        // hacer raycast corto o trigger collider
        // aplicar daño
    }

    public void FinishSwing()
    {
        isSwinging = false;
    }


}