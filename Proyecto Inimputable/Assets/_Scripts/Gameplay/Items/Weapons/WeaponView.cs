using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [Header("Weapon UI")]
    public WeaponStats weaponStats;
    [Header("FX")]
    public Light muzzleFlashLight;
    [Header("WeaponModel")]
    public WeaponModel weaponModel;

    void Start()
    {
        WeaponController.instance.WeaponGameStartState += DisableAllWeapons;
    }

    public void DisableAllWeapons()
    {
        print("disabling all view of weapons");
        if (weaponModel.pistol) weaponModel.pistol.gameObject.SetActive(false);
        if (weaponModel.rifle) weaponModel.rifle.gameObject.SetActive(false);
        if (weaponModel.cane) weaponModel.cane.gameObject.SetActive(false);
        
        if (weaponStats != null) weaponStats.ResetStats();
    }

    public void EquipWeapon(WeaponType type)
    {
        // Disable current weapon
        if (weaponModel.currentWeapon != WeaponType.NONE)
        {
            weaponModel.currentWeapon = WeaponType.NONE;
        }

        weaponModel.currentWeaponObject = null;

        // Enable new weapon
        switch (type)
        {
            case WeaponType.Pistol:
                if (weaponModel.pistol != null && weaponModel.pickedUpWeapons.Contains(WeaponType.Pistol))
                {
                    weaponModel.currentWeapon = WeaponType.Pistol;
                    weaponModel.currentWeaponObject = weaponModel.pistol;
                    weaponModel.pistol.gameObject.SetActive(true);
                    if (weaponModel.cane) weaponModel.cane.gameObject.SetActive(false);
                    if (weaponModel.rifle) weaponModel.rifle.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Rifle:
                if (weaponModel.rifle != null && weaponModel.pickedUpWeapons.Contains(WeaponType.Rifle))
                {
                    weaponModel.currentWeapon = WeaponType.Rifle;
                    weaponModel.currentWeaponObject = weaponModel.rifle;
                    weaponModel.rifle.gameObject.SetActive(true);
                    if (weaponModel.cane) weaponModel.cane.gameObject.SetActive(false);
                    if (weaponModel.pistol) weaponModel.pistol.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Cane:
                if (weaponModel.cane != null && weaponModel.pickedUpWeapons.Contains(WeaponType.Cane))
                {
                    weaponModel.currentWeapon = WeaponType.Cane;
                    //weaponModel.currentWeaponObject = weaponModel.cane;
                    weaponModel.cane.gameObject.SetActive(true);
                    if (weaponModel.rifle) weaponModel.rifle.gameObject.SetActive(false);
                    if (weaponModel.pistol) weaponModel.pistol.gameObject.SetActive(false);
                }
                break;
        }
        weaponModel.currentWeaponObject?.SetState(WeaponState.Drawing);
    }

    public void EnableFlash()
    {
        muzzleFlashLight.gameObject.SetActive(true);
    }

    public void DisableFlash()
    {
        muzzleFlashLight.gameObject.SetActive(false);
    }
    
    
    public void ChangeUiCurrentAmmo()
    {
        weaponStats.SetCurrentAmmo(weaponModel.currentWeaponObject.currentAmmo);
    }

    public void ChangeUiAmmoReserve()
    {
        weaponStats.SetAmmoReserve(weaponModel.currentWeaponObject.ammoReserve);
    }

    public void AfterReloadChangeUi()
    {
        weaponStats.SetCurrentAmmo(weaponModel.currentWeaponObject.currentAmmo);
        weaponStats.SetAmmoReserve(weaponModel.currentWeaponObject.ammoReserve);
    }

}
