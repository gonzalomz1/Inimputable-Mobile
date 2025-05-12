using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Slots")]
    [SerializeField] private WeaponObject melee;
    [SerializeField] private WeaponObject pistol;
    [SerializeField] private WeaponObject rifle;

    [Header("FX")]
    [SerializeField] private Light muzzleFlashLight;

    [Header("WeaponData")]
    [SerializeField] WeaponData pistolData;
    [Header("HashSet for available weapons")]
    public HashSet<WeaponType> pickedUpWeapons = new HashSet<WeaponType>();
    
    [Header("Current Weapon")]
    private WeaponType currentWeapon;

    void Start()
    {
        DisableAllWeapons();
        InitializeWeapons(pistolData, null, null);
    }

    private void DisableAllWeapons()
    {
        if (pistol) pistol.gameObject.SetActive(false);
        if (rifle) rifle.gameObject.SetActive(false);
        if (melee) melee.gameObject.SetActive(false);
    }

    public void InitializeWeapons(WeaponData pistolData, WeaponData rifleData, WeaponData meleeData)
    {
        // Instantiate but disable at first
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

        if (meleeData != null)
        {
            Debug.Log("Initializing Melee");
            melee.Initialize(meleeData, melee.transform);
        }
    }

    public void EquipWeapon(WeaponType type)
    {
        // Disable current weapon
        if (currentWeapon != WeaponType.NONE)
        {
            currentWeapon = WeaponType.NONE;
        }

        WeaponObject newCurrentWeapon = null;

        // Enable new weapon
        switch (type)
        {
            case WeaponType.Pistol:
                if (pistol != null && pickedUpWeapons.Contains(WeaponType.Pistol))
                {
                    currentWeapon = WeaponType.Pistol;
                    newCurrentWeapon = pistol;
                    pistol.gameObject.SetActive(true);
                    if (melee) melee.gameObject.SetActive(false);
                    if (rifle) rifle.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Rifle:
                if (rifle != null && pickedUpWeapons.Contains(WeaponType.Rifle))
                {
                    currentWeapon = WeaponType.Rifle;
                    newCurrentWeapon = rifle;
                    rifle.gameObject.SetActive(true);
                    if (melee) melee.gameObject.SetActive(false);
                    if (pistol) pistol.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Melee:
                if (melee != null && pickedUpWeapons.Contains(WeaponType.Melee))
                {
                    currentWeapon = WeaponType.Melee;
                    newCurrentWeapon = melee;
                    melee.gameObject.SetActive(true);
                    if (rifle) rifle.gameObject.SetActive(false);
                    if (pistol) pistol.gameObject.SetActive(false);
                }
                break;
        }
        newCurrentWeapon?.SetState(WeaponState.Drawing);
    }

    public void PickUpWeapon(WeaponType type)
    {
        if (!pickedUpWeapons.Contains(type))
        {
            pickedUpWeapons.Add(type);
            Debug.Log($"Weapon {type} picked up!");
        }
    }

    public void TryShoot()
    {
        Debug.Log("TryShoot() called.");
    }

    public void TryReload()
    {
        Debug.Log("TryReload() called.");
    }

    public void TriggerPull(WeaponBehaviour weapon)
    {
        weapon?.TriggerPull();
    }


    public void TriggerRelease(WeaponBehaviour weapon)
    {
        weapon?.TriggerRelease();
    }

    public void Reload(WeaponBehaviour weapon)
    {
        weapon?.Reload();
    }

    public WeaponType GetCurrentWeaponType(WeaponBehaviour weapon)
    {
        return weapon.GetWeaponType();
    }

}
