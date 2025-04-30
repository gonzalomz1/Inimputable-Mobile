using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : WeaponBase 
{
// Representa el modelo del arma (datos)

public bool isFiring;
public bool isAutomatic;
public int current_ammo;
public int ammo;

public string projectileType = "Raycast"; // Could be projectile also (maybe)

}