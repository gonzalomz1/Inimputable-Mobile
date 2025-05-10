using Unity.VisualScripting;
using UnityEngine;

public class WeaponObject : WeaponBehaviour 
{
    /*
        in WeaponData, we have info such as:
        - weaponName (string)
        - fireRate (float)
        - damage (int)
        - maxAmmo (int)
        - clipSize (int)
        - isAutomatic (bool)
    */
    private int currentAmmo;
    private int ammoReserve;
    [SerializeField] private Animator animator;
    private float lastShootTime;

    public override void Initialize(WeaponData data, Transform firePoint)
    {
        base.Initialize(data, firePoint);
        currentAmmo = weaponData.clipSize;
        animator = GetComponentInChildren<Animator>();
    }

    public void SetState(WeaponState state)
    {
        switch(state)
        {
            case WeaponState.Drawing:
            animator.SetTrigger("Draw");
            break;
            case WeaponState.Shooting:
            animator.SetTrigger("Shoot");
            break;
            case WeaponState.Empty:
            animator.SetTrigger("Empty");
            break;
            case WeaponState.Reloading:
            animator.SetTrigger("Reload");
            break;
            case WeaponState.Idle:
            animator.SetTrigger("Idle");
            break;
        }
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

    public override void TriggerRelease(){} 

    private void Shoot()
    {
        SetState(WeaponState.Shooting);
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
        currentAmmo = weaponData.clipSize;
        ammoReserve -= weaponData.clipSize;
        Debug.Log("Pistol reloaded.");
    }


    public void FinishReload()
    {
        currentAmmo = weaponData.clipSize;
        SetState(WeaponState.Idle);
    }


}