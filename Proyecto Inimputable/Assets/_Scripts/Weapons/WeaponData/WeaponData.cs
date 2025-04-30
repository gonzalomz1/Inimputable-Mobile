using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : WeaponBase 
{
// Representa el modelo del arma (datos)
/// <summary>
/// necesitamos datos como : 
/// id, listo
/// nombre del arma, listo
/// descripcion, listo
/// capacidad cargador, listo
/// municion maxima en reserva, listo
/// municion actual,listo
/// velocidad de disparo, listo
/// esta disparando?, listo
/// es automatica?,listo
/// url textura
/// </summary>
public int id;
public string weaponName;
public string description;
public string textureUrl;
public int currentAmmo;
public int ammoCapacity;
public int ammoReserve;
public float fireRate;
public bool isFiring;
public bool isAutomatic;
public string projectileType = "Raycast"; // Could be projectile also (maybe)

}