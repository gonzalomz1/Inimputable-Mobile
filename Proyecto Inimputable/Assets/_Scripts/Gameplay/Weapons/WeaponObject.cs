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
    public int currentAmmo;
    public int ammoReserve;
    [SerializeField] private Animator animator;
    private bool isShoting = false;
    private bool isReloading = false;
    public LayerMask RaycastLayers;


    public override void Initialize(WeaponData data, Transform firePoint)
    {
        base.Initialize(data, firePoint);
        currentAmmo = weaponData.clipSize;
        ammoReserve = weaponData.maxAmmo;
        animator = GetComponentInChildren<Animator>();
    }

    public void SetState(WeaponState state)
    {
        switch (state)
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
                isReloading = true;
                break;
            case WeaponState.Idle:
                CheckEmptyAmmo();
                break;
        }
    }

    public override bool TriggerPull()
    {
        if (isReloading) return false;
        if (currentAmmo <= 0)
        {
            SetState(WeaponState.Empty);
            return false;
        }

        if (!isShoting)
        {
            Shoot();
            return true;
        }

        return false; // avoid error
    }

    public override void TriggerRelease() { }

    private void Shoot()
    {
        SetState(WeaponState.Shooting);
        isShoting = true;
        FireRaycast();
        currentAmmo--;
        Debug.Log("Pistol fired. Remaining ammo: " + currentAmmo);
    }


    private void FireRaycast()
    {
        Camera camera = Camera.main;
        Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 100f, RaycastLayers);
        Debug.Log($"hit es: {hit}");
        Debug.Log($"rigidbody: {hit.rigidbody}");
        if (hit.rigidbody != null && hit.transform.tag == "Enemies")
        {
            hit.transform.GetComponent<TurroModel>().TakeDamage(weaponData.damage);
        }
    }

    public void EnableFlash()
    {
        FindWeaponView().EnableFlash();
    }

    public void DisableFlash()
    {
        FindWeaponView().DisableFlash();
    }

    public void FinishShooting()
    {
        isShoting = false;
        BackToIdle();
    }

    public void BackToIdle()
    {
        animator.SetTrigger("ReturnIdle");
    }

    public override void Reload()
    {
        if (CanReload())
        {
            SetState(WeaponState.Reloading);
        }
    }

    public bool CanReload()
    {
        return currentAmmo < weaponData.clipSize && ammoReserve > 0 && !isReloading;
    }

    public void OnReloadAnimationFinish()
    {
        int ammoToReload = CheckAmmoReserve();
        currentAmmo = ammoToReload;
        ammoReserve -= ammoToReload;

        FindWeaponView().AfterReloadChangeUi();
        isReloading = false;
        SetState(WeaponState.Idle);
        Debug.Log("Reloaded.");

    }

    public int CheckAmmoReserve()
    {
        int newAmmo;
        if (ammoReserve < weaponData.clipSize) newAmmo = ammoReserve;
        else newAmmo = weaponData.clipSize;
        return newAmmo;
    }

    public void CheckEmptyAmmo()
    {
        if (currentAmmo == 0) Reload();
    }

    public WeaponController FindWeaponController()
    {
        GameObject player = GameObject.FindWithTag("Player");
        WeaponController wc = player.GetComponent<WeaponController>();
        return wc;
    }
    
    public WeaponModel FindWeaponModel()
    {
        GameObject player = GameObject.FindWithTag("Player");
        WeaponModel wc = player.GetComponent<WeaponModel>();
        return wc;
    }
    
        public WeaponView FindWeaponView()
    {
        GameObject player = GameObject.FindWithTag("Player");
        WeaponView wc = player.GetComponent<WeaponView>();
        return wc;
    }
    
}