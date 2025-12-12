using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [Header("Weapon Slots")]
    public MeleeWeaponObject cane;
    public WeaponObject pistol;
    public WeaponObject rifle;
    [Header("WeaponData")]
    public WeaponDataSO pistolData;
    public WeaponDataSO rifleData;
    public WeaponDataSO caneData;
    [Header("HashSet for available weapons")]
    public HashSet<WeaponType> pickedUpWeapons = new HashSet<WeaponType>();

    [Header("Current Weapon")]
    public WeaponType currentWeapon;
    public WeaponObject currentWeaponObject;

    public bool isMeleeDraw = false;

    void Start()
    {
        WeaponController.instance.WeaponGameStartState += OnWeaponGameStartState;
    }

    private void OnWeaponGameStartState()
    {
        if (caneData != null)
        {
            Debug.Log("Initializing Cane");
            cane.Initialize(caneData, cane.transform);
        }

        if (pistolData != null)
        {
            Debug.Log("Initializing Pistol");
            pistol.Initialize(pistolData, pistol.transform);
        }
        if (rifleData != null)
        {
            Debug.Log("Initializing Rifle");
            rifle.Initialize(rifleData, rifle.transform);
        }
    }

    public WeaponType GetCurrentWeaponType(WeaponBehaviour weapon)
    {
        return weapon.GetWeaponType();
    }

    public WeaponObject GetCurrentWeapon()
    {
        if (currentWeapon == WeaponType.Pistol) return pistol;
        if (currentWeapon == WeaponType.Rifle) return rifle;
        Debug.LogError("No value for currentWeapon on WeaponController");
        return null;

    }

    public void ResetWeaponState()
    {
        pickedUpWeapons.Clear();
        currentWeapon = WeaponType.NONE;
        currentWeaponObject = null;
        
        // Re-initialize to reset stats (ammo)
        OnWeaponGameStartState();
    }
}