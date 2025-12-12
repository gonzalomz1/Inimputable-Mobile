using Unity.VisualScripting;
using UnityEngine;

public class WeaponObject : WeaponBehaviour
{
    public int currentAmmo;
    public int ammoReserve;
    [SerializeField] private Animator animator;
    private bool isShoting = false;
    private bool isReloading = false;
    private bool needAmmo = true;
    private bool needToReload = true;
 
    public WeaponType weaponType;


    public override void Initialize(WeaponDataSO data, Transform firePoint)
    {
        base.Initialize(data, firePoint);
        currentAmmo = weaponDataSO.clipSize;
        ammoReserve = weaponDataSO.maxAmmo;
        animator = GetComponentInChildren<Animator>();
        
        // Fix for Retry Bug: Reset state flags
        isShoting = false;
        isReloading = false;
        needAmmo = true; 
        needToReload = true;
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
        Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 100f, GameData.instance.playerHitMask);
        Debug.Log($"hit es: {hit}");
        Debug.Log($"rigidbody: {hit.rigidbody}");
        if (hit.rigidbody != null && hit.transform.tag == "Enemies")
        {
            hit.transform.GetComponent<TurroModel>().TakeDamage(weaponDataSO.damage);
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
        return weaponType != WeaponType.Cane && currentAmmo < weaponDataSO.clipSize && ammoReserve > 0 && !isReloading;
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
        if (ammoReserve < weaponDataSO.clipSize) newAmmo = ammoReserve;
        else newAmmo = weaponDataSO.clipSize;
        return newAmmo;
    }

    public void AddAmmo(int amount)
    {
        Debug.Log($"[WeaponObject] AddAmmo called on {name}. Amount: {amount}. Old Reserve: {ammoReserve}");
        ammoReserve += amount;
        Debug.Log($"[WeaponObject] New Reserve: {ammoReserve}");
        
        // Update the AMMO RESERVE UI, not just the current clip
        WeaponView view = FindWeaponView();
        if (view != null)
        {
            view.ChangeUiAmmoReserve();
        }
        else
        {
             Debug.LogError("[WeaponObject] Could not find WeaponView to update UI!");
        }
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