using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("WeaponModel")]
    public WeaponModel weaponModel;
    [Header("WeaponView")]
    public WeaponView weaponView;
    
    public void StartWeapons()
    {
        weaponView.DisableAllWeapons();
        InitializeWeapons(weaponModel.pistolData, null, null);
    }



    public void InitializeWeapons(WeaponData pistolData, WeaponData rifleData, WeaponData meleeData)
    {
        // Instantiate but disable at first
        if (weaponModel.pistolData != null)
        {
            Debug.Log("Initializing Pistol");
            weaponModel.pistol.Initialize(pistolData, weaponModel.pistol.transform);
        }

        if (weaponModel.rifleData != null)
        {
            Debug.Log("Initializing Rifle");
            weaponModel.rifle.Initialize(rifleData, weaponModel.rifle.transform);
        }

        if (weaponModel.meleeData != null)
        {
            Debug.Log("Initializing Melee");
            weaponModel.melee.Initialize(meleeData, weaponModel.melee.transform);
        }
    }

    

    public void PickUpWeapon(WeaponType type)
    {
        if (!weaponModel.pickedUpWeapons.Contains(type))
        {
            weaponModel.pickedUpWeapons.Add(type);
            weaponView.EquipWeapon(WeaponType.Pistol);
            Debug.Log($"Weapon {type} picked up!");
        }
        OnNewWeaponStats();
    }

    public void OnNewWeaponStats()
    {
        Debug.Log($"Current Weapon: {weaponModel.currentWeapon}");
        Debug.Log("on Change UI Stats");
        WeaponObject newWeapon = weaponModel.GetCurrentWeapon();
        Debug.Log($"after call, newWeapon is: {newWeapon}");
        weaponView.weaponStats.NewWeaponEquiped(newWeapon);
    }


    public void TryShoot() // called onClick() from ActionCanvas:ShootButton
    {
        if (weaponModel.currentWeaponObject != null)
        {
            bool needToChange = weaponModel.currentWeaponObject.TriggerPull();
            if (needToChange) 
            {
            weaponView.ChangeUiCurrentAmmo();
            }
            else return;
        }
    }

    public void TryReload() // called onClick() from ActionCanvas:ReloadButton
    {
        if (weaponModel.currentWeaponObject != null)
        {
            weaponModel.currentWeaponObject.Reload();
        }
    }

}
