using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [Header("Weapon Slots")]
    public WeaponObject cane;
    public WeaponObject pistol;
    public WeaponObject rifle;
    [Header("WeaponData")]
    public WeaponData pistolData;
    public WeaponData rifleData;
    public WeaponData caneData;
    [Header("HashSet for available weapons")]
    public HashSet<WeaponType> pickedUpWeapons = new HashSet<WeaponType>();

    [Header("Current Weapon")]
    public WeaponType currentWeapon;
    public WeaponObject currentWeaponObject;

        public WeaponType GetCurrentWeaponType(WeaponBehaviour weapon)
    {
        return weapon.GetWeaponType();
    }

    public WeaponObject GetCurrentWeapon()
    {
        if (currentWeapon == WeaponType.Pistol) return pistol;
        if (currentWeapon == WeaponType.Rifle) return rifle;
        if (currentWeapon == WeaponType.Cane) return cane;
        Debug.LogError("No value for currentWeapon on WeaponController");
        return null;

    }
}