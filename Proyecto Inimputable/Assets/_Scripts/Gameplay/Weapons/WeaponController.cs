using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Slots")]
    [SerializeField] private Transform pistolSlot;
    [SerializeField] private Transform rifleSlot;
    [SerializeField] private Transform meleeSlot;

    [Header("FX")]
    [SerializeField] private Light muzzleFlashLight;

    private WeaponBehaviour currentWeapon;
    private WeaponBehaviour pistol;
    private WeaponBehaviour rifle;
    private WeaponBehaviour melee;

    public void InitializeWeapons(WeaponData pistolData, WeaponData rifleData, WeaponData meleeData)
    {
        // Instantiate but disable at first
        if (pistolData != null)
        {
            pistol = Instantiate(pistolData.weaponPrefab, pistolSlot).GetComponent<WeaponBehaviour>();
            pistol.Initialize(pistolData, pistolSlot);
            pistol.gameObject.SetActive(false);
        }

        if (rifleData != null)
        {
            rifle = Instantiate(rifleData.weaponPrefab, rifleSlot).GetComponent<WeaponBehaviour>();
            rifle.Initialize(rifleData, rifleSlot);
            rifle.gameObject.SetActive(false);
        }

        if (meleeData != null)
        {
            melee = Instantiate(meleeData.weaponPrefab, meleeSlot).GetComponent<WeaponBehaviour>();
            melee.Initialize(meleeData, meleeSlot);
            melee.gameObject.SetActive(false);
        }
    }

    public void EquipWeapon(WeaponType type)
    {
        // Disable current weapon
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
            currentWeapon.SetState(WeaponState.Inactive);
        }

        // Enable new weapon
        switch(type)
        {
            case WeaponType.Pistol:
                if (pistol !=null)
                {
                    currentWeapon = pistol;
                    pistol.gameObject.SetActive(true);
                    if (melee) melee.gameObject.SetActive(false);
                    if (rifle) rifle.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Rifle:
                if (rifle !=null)
                {
                    currentWeapon = rifle;
                    rifle.gameObject.SetActive(true);
                    if (melee) melee.gameObject.SetActive(false);
                    if (pistol) pistol.gameObject.SetActive(false);
                }
                break;
            case WeaponType.Melee:
                if (melee !=null)
                {
                    currentWeapon = melee;
                    melee.gameObject.SetActive(true);
                    if (rifle) rifle.gameObject.SetActive(false);
                    if (pistol) pistol.gameObject.SetActive(false);
                }
                break;
        }
        currentWeapon?.SetState(WeaponState.Drawing);
    }

    public void TryShoot(){}

    public void TriggerPull()
    {
        currentWeapon?.TriggerPull();
    }


    public void TriggerRelease()
    {
        currentWeapon?.TriggerRelease();
    }

    public void Reload()
    {
        currentWeapon?.Reload();
    }

    public WeaponType GetCurrentWeaponType()
    {
        return currentWeapon.GetWeaponType();
    }

}
