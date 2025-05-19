using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [Header("Weapon UI")]
    public WeaponStats weaponStats;
    [Header("FX")]
    public Light muzzleFlashLight;
    [Header("WeaponModel")]
    public WeaponModel weaponModel;

    public void DisableAllWeapons()
    {
        if (weaponModel.pistol) weaponModel.pistol.gameObject.SetActive(false);
        if (weaponModel.rifle) weaponModel.rifle.gameObject.SetActive(false);
        if (weaponModel.melee) weaponModel.melee.gameObject.SetActive(false);
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
                    if (weaponModel.melee) weaponModel.melee.gameObject.SetActive(false);
                    if (weaponModel.rifle) weaponModel.rifle.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Rifle:
                if (weaponModel.rifle != null && weaponModel.pickedUpWeapons.Contains(WeaponType.Rifle))
                {
                    weaponModel.currentWeapon = WeaponType.Rifle;
                    weaponModel.currentWeaponObject = weaponModel.rifle;
                    weaponModel.rifle.gameObject.SetActive(true);
                    if (weaponModel.melee) weaponModel.melee.gameObject.SetActive(false);
                    if (weaponModel.pistol) weaponModel.pistol.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Melee:
                if (weaponModel.melee != null && weaponModel.pickedUpWeapons.Contains(WeaponType.Melee))
                {
                    weaponModel.currentWeapon = WeaponType.Melee;
                    weaponModel.currentWeaponObject = weaponModel.melee;
                    weaponModel.melee.gameObject.SetActive(true);
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
