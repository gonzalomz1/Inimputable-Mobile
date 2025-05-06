using UnityEngine;

public class Pistol : WeaponBehaviour
{
    private int currentAmmo;
    [SerializeField] private Animator animator;
    private float lastShootTime;

    public override void Initialize(WeaponData data, Transform firePoint)
    {
        base.Initialize(data, firePoint);
        currentAmmo = weaponData.clipSize;
        animator = GetComponentInChildren<Animator>();
    }

    public override void TriggerPull()
    {
        if (Time.time < lastShootTime + 1f / weaponData.fireRate) return;

        if (currentAmmo > 0)
        {
            SetState(WeaponState.Empty);
            return;
        }

        Shoot();
    }

    public override void TriggerRelease(){} // Pistol is not automatic (don't do anything)

    private void Shoot()
    {
        SetState(WeaponState.Firing);
        currentAmmo--;
        lastShootTime = Time.time;
        animator?.SetTrigger("Shoot");
        Debug.Log("Pistol fired. Remaining ammo: " + currentAmmo);
        // here we can enable raycast
        SetState(currentAmmo > 0 ? WeaponState.Idle : WeaponState.Empty);
       
    }

    public override void Reload()
    {
        if (currentAmmo < weaponData.clipSize)
        {
            SetState(WeaponState.Reloading);
            animator?.SetTrigger("Reload");
            // CALLING at the end of animation FinishReload() from PistolSprite class(component of PistolSprite)
        }
        // trigger de animacion reload?
        // preguntar si hay balas restantes por recargar?
        currentAmmo = weaponData.clipSize;
        Debug.Log("Pistol reloaded.");
    }


    public void FinishReload()
    {
        currentAmmo = weaponData.clipSize;
        SetState(WeaponState.Idle);
    }


}
