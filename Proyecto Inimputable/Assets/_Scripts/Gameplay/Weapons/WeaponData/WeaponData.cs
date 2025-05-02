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
/// daño
/// url textura
/// </summary>
public int id;
public string weaponName;
public string description;
public string textureUrl; // Weapon visual
public Sprite sprite;
public Material spriteMaterial; // 3D World sprite or UI
public int currentAmmo;
public int ammoCapacity;
public int ammoReserve;
public int maxAmmoReserve;
public int ammoPickupSize;
public float fireRate;
public bool isFiring;
public bool isAutomatic;
public int damage;
public string projectileType = "Raycast"; // Could be projectile also (maybe)

}