using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
public string weaponName;
public float fireRate;
public int damage;
public int maxAmmo = 120;
public int clipSize;
public bool isAutomatic;
public WeaponType weaponType;
public float reloadTime;
public GameObject weaponPrefab;
public Sprite weaponIcon;
}


