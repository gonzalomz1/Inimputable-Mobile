using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStats : MonoBehaviour
{
    [Header("Text On UI")]
    public TextMeshProUGUI currentAmmo;
    public TextMeshProUGUI ammoReserve;


    public void NewWeaponEquiped(WeaponObject newWeapon)
    {
        Debug.Log($"New weapon current ammo: {newWeapon.currentAmmo}");
        Debug.Log($"New weapon ammo reserve: {newWeapon.ammoReserve}");
        SetCurrentAmmo(newWeapon.currentAmmo);
        SetAmmoReserve(newWeapon.ammoReserve);
    }

    public void SetCurrentAmmo(int amount)
    {
        currentAmmo.text = $"{amount}";
    }

    public void SetAmmoReserve(int amount)
    {
        ammoReserve.text = $"{amount}";
    }
}
