using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon UI")]
    public WeaponStats weaponStats;
    [Header("Weapon Slots")]
    [SerializeField] private WeaponObject melee;
    [SerializeField] private WeaponObject pistol;
    [SerializeField] private WeaponObject rifle;
    [Header("FX")]
    [SerializeField] private Light muzzleFlashLight;
    public bool flashing = false;
    [Header("WeaponData")]
    [SerializeField] WeaponData pistolData;
    [Header("HashSet for available weapons")]
    public HashSet<WeaponType> pickedUpWeapons = new HashSet<WeaponType>();
    [Header("Current Weapon")]
    private WeaponType currentWeapon;
    private WeaponObject currentWeaponObject;


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

        currentWeaponObject = null;

        // Enable new weapon
        switch (type)
        {
            case WeaponType.Pistol:
                if (pistol != null && pickedUpWeapons.Contains(WeaponType.Pistol))
                {
                    currentWeapon = WeaponType.Pistol;
                    currentWeaponObject = pistol;
                    pistol.gameObject.SetActive(true);
                    if (melee) melee.gameObject.SetActive(false);
                    if (rifle) rifle.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Rifle:
                if (rifle != null && pickedUpWeapons.Contains(WeaponType.Rifle))
                {
                    currentWeapon = WeaponType.Rifle;
                    currentWeaponObject = rifle;
                    rifle.gameObject.SetActive(true);
                    if (melee) melee.gameObject.SetActive(false);
                    if (pistol) pistol.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Melee:
                if (melee != null && pickedUpWeapons.Contains(WeaponType.Melee))
                {
                    currentWeapon = WeaponType.Melee;
                    currentWeaponObject = melee;
                    melee.gameObject.SetActive(true);
                    if (rifle) rifle.gameObject.SetActive(false);
                    if (pistol) pistol.gameObject.SetActive(false);
                }
                break;
        }
        currentWeaponObject?.SetState(WeaponState.Drawing);
    }

    public void PickUpWeapon(WeaponType type)
    {
        if (!pickedUpWeapons.Contains(type))
        {
            pickedUpWeapons.Add(type);
            EquipWeapon(WeaponType.Pistol);
            Debug.Log($"Weapon {type} picked up!");
        }
        OnNewWeaponStats();
    }

    public void OnNewWeaponStats()
    {
        Debug.Log($"Current Weapon: {currentWeapon}");
        Debug.Log("on Change UI Stats");
        WeaponObject newWeapon = GetCurrentWeapon();
        Debug.Log($"after call, newWeapon is: {newWeapon}");
        weaponStats.NewWeaponEquiped(newWeapon);
    }


    public void TryShoot() // called onClick() from ActionCanvas:ShootButton
    {
        if (currentWeaponObject != null)
        {
            bool needToChange = currentWeaponObject.TriggerPull();
            if (needToChange) 
            {
            ChangeUiCurrentAmmo();
            }
            else return;
        }
    }

    public void TryReload() // called onClick() from ActionCanvas:ReloadButton
    {
        if (currentWeaponObject != null)
        {
            currentWeaponObject.Reload();
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
        if (currentWeapon == WeaponType.Melee) return melee;
        Debug.LogError("No value for currentWeapon on WeaponController");
        return null;

    }

    public void ChangeUiCurrentAmmo()
    {
        weaponStats.SetCurrentAmmo(currentWeaponObject.currentAmmo);
    }

    public void ChangeUiAmmoReserve()
    {
        weaponStats.SetAmmoReserve(currentWeaponObject.ammoReserve);
    }

    public void AfterReloadChangeUi()
    {
        weaponStats.SetCurrentAmmo(currentWeaponObject.currentAmmo);
        weaponStats.SetAmmoReserve(currentWeaponObject.ammoReserve);
    }

    void Update()
    {
        muzzleFlashLight.gameObject.SetActive(flashing);
    }

}
